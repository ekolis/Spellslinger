using Spellslinger.Services;
using static System.Net.Mime.MediaTypeNames;

namespace Spellslinger.Models.Spells;

public record AirVortex
	: Spell
{
	public AirVortex()
	{
		Stats = new SpellStats(
			Name: x => $"{x.Element} Vortex",
			Description: x => $"Blasts a nearby enemy with {x.Element.Description} and tries to knock them back.",
			Details: x => "Damage and knockback chance scale with willpower, range with memory.",
			MPCost: 6,
			Element: Element.Air,
			Power: x => x.Willpower,
			Knockback:  x => x.Willpower / 2,
			Range: x => x.Memory);
	}

	public override bool IsDirectional => true;

	protected override void CastImpl(IGame game, Actor caster, int dx, int dy)
	{
		var casterPos = game.CurrentMap.LocateActor(caster);
		if (dx == 0 && dy == 0)
		{
			game.Log.Add($"But the {caster} wisely aborts the spell to avoid casting it at themselves.");
		}
		if (dx == 0)
		{
			// casting spell vertically
			// cast one bolt
			var xpos = casterPos.x;
			int distance = 1;
			bool hitSomething = false;
			while (!hitSomething && distance <= Stats.Range(caster.Stats))
			{
				// TODO: display the ray in the UI
				var ypos = casterPos.y + distance * Math.Sign(dy);
				if (xpos < 0 || xpos >= game.CurrentMap.Width || ypos < 0 || ypos >= game.CurrentMap.Height)
				{
					// the bolt is off the map
					hitSomething = true;
				}
				else if (game.CurrentMap.Tiles[xpos, ypos].Actor is not null)
				{
					// the bolt hit an actor
					HitTile(game, caster, Stats.Power(caster.Stats), Stats.Knockback(caster.Stats), xpos, ypos, dx, dy);
					hitSomething = true;
				}
				else if (!game.CurrentMap.Tiles[xpos, ypos].Terrain.IsPassable)
				{
					// the bolt hit a wall
					hitSomething = true;
				}

				// move the bolt
				distance++;
			}
		}
		else if (dy == 0)
		{
			// casting spell horizontally
			// cast one bolt
			var ypos = casterPos.y;
			int distance = 1;
			bool hitSomething = false;
			while (!hitSomething && distance <= Stats.Range(caster.Stats))
			{
				// TODO: display the ray in the UI
				var xpos = casterPos.x + distance * Math.Sign(dx);
				if (xpos < 0 || xpos >= game.CurrentMap.Width || ypos < 0 || ypos >= game.CurrentMap.Height)
				{
					// the bolt is off the map
					hitSomething = true;
				}
				else if (game.CurrentMap.Tiles[xpos, ypos].Actor is not null)
				{
					// the bolt hit an actor
					HitTile(game, caster, Stats.Power(caster.Stats), Stats.Knockback(caster.Stats), xpos, ypos, dx, dy);
					hitSomething = true;
				}
				else if (!game.CurrentMap.Tiles[xpos, ypos].Terrain.IsPassable)
				{
					// the bolt hit a wall
					hitSomething = true;
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
}
