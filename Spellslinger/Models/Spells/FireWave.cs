using Spellslinger.Services;

namespace Spellslinger.Models.Spells;

public class FireWave
	: Spell
{
	public FireWave(IGame game)
		: base(game)
	{
	}

	public override string Name => "Fire Wave";
	public override string Description => "Blasts nearby enemies in one direction with searing flames.";
	public override string Details => "Damage scales with willpower, range with memory.";
	public override bool IsDirectional => true;
	public override int MPCost => 8;

	protected override void CastImpl(Actor caster, int dx, int dy)
	{
		var casterPos = Game.CurrentMap.LocateActor(caster);
		if (dx == 0 && dy == 0)
		{
			Game.Log.Add($"But the {caster} wisely aborts the spell to avoid casting it at themselves.");
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
					if (rayXPosition < 0 || rayXPosition >= Game.CurrentMap.Width || rayYPosition < 0 || rayYPosition >= Game.CurrentMap.Height)
					{
						// ray is off the map
						rayXPositions.Remove(rayXPosition);
					}

					// let the ray inflict damage
					HitTile(caster.Stats.Willpower * 2, rayXPosition, rayYPosition);

					// TODO: display the ray in the UI

					if (!Game.CurrentMap.Tiles[rayXPosition, rayYPosition].Terrain.IsPassable)
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
					if (rayXPosition < 0 || rayXPosition >= Game.CurrentMap.Width || rayYPosition < 0 || rayYPosition >= Game.CurrentMap.Height)
					{
						// ray is off the map
						rayYPositions.Remove(rayYPosition);
					}

					// let the ray inflict damage
					HitTile(caster.Stats.Willpower * 2, rayXPosition, rayYPosition);

					// TODO: display the ray in the UI

					if (!Game.CurrentMap.Tiles[rayXPosition, rayYPosition].Terrain.IsPassable)
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
			Game.Log.Add($"But {Name} can't be cast diagonally.");
		}
	}

	private void HitTile(int damage, int rayXPosition, int rayYPosition)
	{
		var targetTile = Game.CurrentMap.Tiles[rayXPosition, rayYPosition];
		if (targetTile.Actor is not null)
		{
			Game.Log.Add($"The flames burn the {targetTile.Actor} ({damage} damage).");
			targetTile.Actor.TakeDamage(damage);
		}
	}
}
