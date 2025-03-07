using System.Xml.Linq;
using Spellslinger.Services;

namespace Spellslinger.Models;

/// <summary>
/// A magic spell.
/// </summary>
public abstract record Spell()
{
	/// <summary>
	/// Basic stats of the spell. Can be affected by modifiers.
	/// </summary>
	public SpellStats Stats { get; init; }

	/// <summary>
	/// The name of the spell.
	/// </summary>
	public string Name => Stats.Name(Stats);

	/// <summary>
	/// A brief description of the spell's effects.
	/// </summary>
	public string Description => Stats.Description(Stats);

	/// <summary>
	/// Additional details on the spell's effects, such as formulas and stats used.
	/// </summary>
	public string Details => Stats.Details(Stats);

	/// <summary>
	/// The number of MP required to cast the spell.
	/// </summary>
	public int MPCost => Stats.MPCost;

	/// <summary>
	/// Is this spell cast in a particular direction?
	/// </summary>
	public abstract bool IsDirectional { get; }

	/// <summary>
	/// Any modifiers that have been applied to this spell.
	/// </summary>
	public IEnumerable<SpellModifier> Modifiers { get; init; } = [];

	/// <summary>
	/// The effect to apply to affected tiles.
	/// </summary>
	public virtual Effect Effect => new Effect { Color = Stats.Element.Color };

	/// <summary>
	/// Casts the spell.
	/// </summary>
	/// <param name="caster">The actor who is casting the spell.</param>
	/// <param name="dx">The x-component of the direction vector (ignored if non-directional).</param>
	/// <param name="dy">The y-component of the direction vector (ignored if non-directional).</param>
	/// <returns>true if a turn was spent casting the spell, false if the spell couldn't be cast (e.g lack of MP).</returns>
	public async Task<bool> Cast(IGame game, Actor caster, int dx, int dy)
	{
		if (caster.MP.Value < MPCost)
		{
			game.Log.Add($"The {caster} tries to cast {Name}, but lacks the required MP.");
			return false;
		}
		else
		{
			game.Log.Add($"The {caster} casts {Name}.");
			caster.MP.Deplete(MPCost);
			if (caster.Mana.Value <= 0)
			{
				game.Log.Add($"The {caster}'s mana is depleted.");
			}
			if (caster.Reserves.Value <= 0)
			{
				game.Log.Add($"The {caster}'s reserves are depleted.");
			}
			CastImpl(game, caster, dx, dy);
			caster.ScheduleNextTurn();

			// update MP bar
			game.Update(caster.Tile);

			// show effect
			await Task.Delay(150);

			// remove effect
			foreach (var tile in AffectedTiles)
			{
				tile.Effect = null;
				game.Update(tile);
			}
			AffectedTiles.Clear();

			return true;
		}
	}

	protected abstract void CastImpl(IGame game, Actor caster, int dx, int dy);

	public Spell ApplyModifier(SpellModifier modifier)
	{
		return this with
		{
			// add the modifier to the list of modifiers on the spell
			Modifiers = [.. Modifiers, modifier],

			// apply the modifier to the spell's stats
			Stats = modifier.Apply(Stats)
		};
	}

	public override string ToString()
	{
		return Name;
	}

	/// <summary>
	/// List of tiles that were just affected by this spell.
	/// </summary>
	private IList<Tile> AffectedTiles { get; } = [];

	protected void HitTile(IGame game, Actor caster, SpellTags tags, int damage, int knockback, int teleport, int xpos, int ypos, int dx, int dy)
	{
		// find target tile and actor
		var targetTile = game.CurrentMap.Tiles[xpos, ypos];
		var target = targetTile.Actor;

		// apply effect
		targetTile.Effect = Effect;
		AffectedTiles.Add(targetTile);
		game.Update(targetTile);

		// apply damage
		if (target is not null)
		{
			// TODO: refactor calculating resists twice
			var modifiedDamage = damage;
			if (target.Resistances.Contains(Stats.Element) && !target.Vulnerabilities.Contains(Stats.Element))
			{
				modifiedDamage /= 2;
			}
			if (target.Vulnerabilities.Contains(Stats.Element) && !target.Resistances.Contains(Stats.Element))
			{
				modifiedDamage *= 3;
				modifiedDamage /= 2;
			}
			game.Log.Add($"The {Stats.Element.Description} {Stats.Element.Verb} the {target} ({modifiedDamage} damage).");
			target.TakeDamage(damage, caster, tags, Stats.Element);
		}

		if (target is not null && game.Rng.NextDouble() < (double)knockback / (knockback + target.Stats.Toughness))
		{
			// apply knockback
			// TODO: keep track of which tiles are also being hit so actors can play musical chairs
			var knockbackXpos = xpos + Math.Sign(dx);
			var knockbackYpos = ypos + Math.Sign(dy);
			if (knockbackXpos >= 0 && knockbackXpos < game.CurrentMap.Width
				&& knockbackYpos >= 0 && knockbackYpos < game.CurrentMap.Height
				&& game.CurrentMap.Tiles[knockbackXpos, knockbackYpos].Actor is null)
			{
				game.CurrentMap.Tiles[knockbackXpos, knockbackYpos].Actor = game.CurrentMap.Tiles[xpos, ypos].Actor;
				game.CurrentMap.Tiles[xpos, ypos].Actor = null;
			}
		}

		if (target is not null && game.Rng.NextDouble() < (double)teleport / (teleport + target.Stats.Memory))
		{
			// apply teleport
			TeleportActor(game, target, teleport);
		}
	}

	protected void CastBolt(IGame game, Actor caster, int dx, int dy)
	{
		if (dx == 0 && dy == 0)
		{
			game.Log.Add($"But the {caster} wisely aborts the spell to avoid casting it at themselves.");
		}
		else if (dx == 0)
		{
			// casting spell vertically
			// cast one bolt
			var xpos = caster.Tile.X;
			int distance = 1;
			bool hitSomething = false;
			while (!hitSomething && distance <= Stats.Range(caster.Stats))
			{
				// TODO: display the ray in the UI
				var ypos = caster.Tile.Y + distance * Math.Sign(dy);
				var targetTile = game.CurrentMap.Tiles[xpos, ypos];
				if (xpos < 0 || xpos >= game.CurrentMap.Width || ypos < 0 || ypos >= game.CurrentMap.Height)
				{
					// the bolt is off the map
					hitSomething = true;
				}
				else if (targetTile.Actor is not null)
				{
					// the bolt hit an actor
					HitTile(game, caster, Stats.Tags, Stats.Power(caster.Stats), Stats.Knockback(caster.Stats), Stats.Teleport(caster.Stats), xpos, ypos, dx, dy);
					hitSomething = true;
				}
				else if (!targetTile.Terrain.IsPassable)
				{
					// the bolt hit a wall
					hitSomething = true;
				}
				else
				{
					// the bolt traveled through the air
					// apply effect
					targetTile.Effect = Effect;
					AffectedTiles.Add(targetTile);
					game.Update(targetTile);
				}

				// move the bolt
				distance++;
			}
		}
		else if (dy == 0)
		{
			// casting spell horizontally
			// cast one bolt
			var ypos = caster.Tile.Y;
			int distance = 1;
			bool hitSomething = false;
			while (!hitSomething && distance <= Stats.Range(caster.Stats))
			{
				// TODO: display the ray in the UI
				var xpos = caster.Tile.X + distance * Math.Sign(dx);
				var targetTile = game.CurrentMap.Tiles[xpos, ypos];
				if (xpos < 0 || xpos >= game.CurrentMap.Width || ypos < 0 || ypos >= game.CurrentMap.Height)
				{
					// the bolt is off the map
					hitSomething = true;
				}
				else if (targetTile.Actor is not null)
				{
					// the bolt hit an actor
					HitTile(game, caster, Stats.Tags, Stats.Power(caster.Stats), Stats.Knockback(caster.Stats), Stats.Teleport(caster.Stats), xpos, ypos, dx, dy);
					hitSomething = true;
				}
				else if (!targetTile.Terrain.IsPassable)
				{
					// the bolt hit a wall
					hitSomething = true;
				}
				else
				{
					// the bolt traveled through the air
					// apply effect
					targetTile.Effect = Effect;
					AffectedTiles.Add(targetTile);
					game.Update(targetTile);
				}

				// move the bolt
				distance++;
			}
		}
		else
		{
			game.Log.Add($"But {Name} can't be cast diagonally.");
		}
	}

	protected void TeleportActor(IGame game, Actor actor, int maxDistance)
	{
		// find nearby tiles starting at max distance and working inward
		for (var distance = maxDistance; distance > 0; distance--)
		{
			IList<Tile> candidates = [];
			if (actor?.Tile is not null)
			{
				for (var x = Math.Max(0, actor.Tile.X - distance); x <= Math.Min(game.CurrentMap.Width - 1, actor.Tile.X + distance); x++)
				{
					for (var y = Math.Max(0, actor.Tile.Y - distance); y <= Math.Min(game.CurrentMap.Height - 1, actor.Tile.Y + distance); y++)
					{
						var targetDistance = Math.Abs(x - actor.Tile.X) + Math.Abs(y - actor.Tile.Y);
						var tile = game.CurrentMap.Tiles[x, y];

						// only consider tiles at the correct distance with passable terrain and no actor present
						if (targetDistance == distance && tile.Terrain.IsPassable && tile.Actor is null)
						{
							candidates.Add(tile);
						}
					}
				}
			}
			if (candidates.Any())
			{
				var candidate = candidates.PickWeighted(q => 1, game.Rng);
				game.Log.Add($"The {actor} teleports!");
				AffectedTiles.Add(actor.Tile);
				game.Update(actor.Tile);
				actor.Tile.Actor = null;
				candidate.Actor = actor;
				AffectedTiles.Add(actor.Tile);
				game.Update(actor.Tile);
				break;
			}
		}
	}
}
