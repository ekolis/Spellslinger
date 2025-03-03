using Spellslinger.Services;

namespace Spellslinger.Models.Spells;

public record FireWave
	: Spell
{
	public FireWave()
	{
		Stats = new SpellStats(
			Name: x => $"{x.Element} Wave",
			Description: x => $"Blasts nearby enemies in one direction with {x.Element.Description}.",
			Details: x => "Damage scales with willpower, range with memory.",
			MPCost: 8,
			Element: Element.Fire,
			Power: x => x.Willpower * 2);
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
			// cast three parallel rays centered on the caster
			// up to a length equal to the caster's memory stat
			List<int> rayXPositions = [casterPos.x - 1, casterPos.x, casterPos.x + 1];
			int rayLength = 1;
			while (rayXPositions.Any() && rayLength <= caster.Stats.Memory)
			{
				var rayYPosition = casterPos.y + rayLength * Math.Sign(dy);
				foreach (var rayXPosition in rayXPositions)
				{
					if (rayXPosition < 0 || rayXPosition >= game.CurrentMap.Width || rayYPosition < 0 || rayYPosition >= game.CurrentMap.Height)
					{
						// ray is off the map
						rayXPositions.Remove(rayXPosition);
					}

					// let the ray inflict damage
					HitTile(game, Stats.Power(caster.Stats), rayXPosition, rayYPosition);

					// TODO: display the ray in the UI

					if (!game.CurrentMap.Tiles[rayXPosition, rayYPosition].Terrain.IsPassable)
					{
						// ray hit a wall
						rayXPositions.Remove(rayXPosition);
					}
				}
				rayLength++;
			}
		}
		else if (dy == 0)
		{
			// casting spell horizontally
			// cast three parallel rays centered on the caster
			// up to a length equal to the caster's memory stat
			List<int> rayYPositions = [casterPos.y - 1, casterPos.y, casterPos.y + 1];
			int rayLength = 1;
			while (rayYPositions.Any() && rayLength <= caster.Stats.Memory)
			{
				var rayXPosition = casterPos.x + rayLength * Math.Sign(dx);
				foreach (var rayYPosition in rayYPositions)
				{
					if (rayXPosition < 0 || rayXPosition >= game.CurrentMap.Width || rayYPosition < 0 || rayYPosition >= game.CurrentMap.Height)
					{
						// ray is off the map
						rayYPositions.Remove(rayYPosition);
					}

					// let the ray inflict damage
					HitTile(game, Stats.Power(caster.Stats), rayXPosition, rayYPosition);

					// TODO: display the ray in the UI

					if (!game.CurrentMap.Tiles[rayXPosition, rayYPosition].Terrain.IsPassable)
					{
						// ray hit a wall
						rayYPositions.Remove(rayYPosition);
					}
				}
				rayLength++;
			}
		}
		else
		{
			game.Log.Add($"But {Name} can't be cast diagonally.");
		}
	}

	private void HitTile(IGame game, int damage, int rayXPosition, int rayYPosition)
	{
		var targetTile = game.CurrentMap.Tiles[rayXPosition, rayYPosition];
		if (targetTile.Actor is not null)
		{
			game.Log.Add($"The flames burn the {targetTile.Actor} ({damage} damage).");
			targetTile.Actor.TakeDamage(damage);
		}
	}
}
