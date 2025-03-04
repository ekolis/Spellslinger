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
	/// Casts the spell.
	/// </summary>
	/// <param name="caster">The actor who is casting the spell.</param>
	/// <param name="dx">The x-component of the direction vector (ignored if non-directional).</param>
	/// <param name="dy">The y-component of the direction vector (ignored if non-directional).</param>
	/// <returns>true if a turn was spent casting the spell, false if the spell couldn't be cast (e.g lack of MP).</returns>
	public bool Cast(IGame game, Actor caster, int dx, int dy)
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

	protected void HitTile(IGame game, Actor caster, SpellTags tags, int damage, int knockback, int xpos, int ypos, int dx, int dy)
	{
		// apply damage
		var targetTile = game.CurrentMap.Tiles[xpos, ypos];
		if (targetTile.Actor is not null)
		{
			// TODO: refactor calculating resists twice
			var modifiedDamage = damage;
			if (targetTile.Actor.Resistances.Contains(Stats.Element) && !targetTile.Actor.Vulnerabilities.Contains(Stats.Element))
			{
				modifiedDamage /= 2;
			}
			if (targetTile.Actor.Vulnerabilities.Contains(Stats.Element) && !targetTile.Actor.Resistances.Contains(Stats.Element))
			{
				modifiedDamage *= 3;
				modifiedDamage /= 2;
			}
			game.Log.Add($"The {Stats.Element.Description} {Stats.Element.Verb} the {targetTile.Actor} ({modifiedDamage} damage.");
			targetTile.Actor.TakeDamage(damage, caster, tags, Stats.Element);
		}

		if (targetTile.Actor is not null && game.Rng.NextDouble() < (double)knockback / (knockback + targetTile.Actor.Stats.Toughness))
		{
			// apply knockback
			// TODO: keep track of which tiles are also being hit so actors can play musical chairs
			var knockbackXpos = xpos + Math.Sign(dx);
			var knockbackYpos = ypos + Math.Sign(dy);
			if (knockbackXpos >= 0 && knockbackXpos < game.CurrentMap.Width
				&& knockbackYpos >= 0 && knockbackYpos < game.CurrentMap.Height
				&& game.CurrentMap.Tiles[knockbackXpos, knockbackYpos].Actor is null)
			{
				game.CurrentMap.Tiles[xpos, knockbackYpos].Actor = game.CurrentMap.Tiles[xpos, ypos].Actor;
				game.CurrentMap.Tiles[xpos, ypos].Actor = null;
			}
		}
	}
}
