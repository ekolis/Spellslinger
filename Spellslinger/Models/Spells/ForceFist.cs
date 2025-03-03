using Spellslinger.Services;

namespace Spellslinger.Models.Spells;

public class ForceFist
	: Spell
{
	public ForceFist()
	{
		Stats = new SpellStats(
			Name: x => $"{x.Element} Fist",
			Description: x => $"Projects a short ranged ethereal fist made of pure {x.Element.Description}.",
			Details: x => "Damage scales with strength and willpower.",
			MPCost: 3,
			Element: Element.Force);
	}

	public override bool IsDirectional => true;

	protected override void CastImpl(IGame game, Actor caster, int dx, int dy)
	{
		var casterPos = game.CurrentMap.LocateActor(caster);
		// TODO: deal with array index out of bounds if we try punching too far
		var targetTile = game.CurrentMap.Tiles[casterPos.x + dx, casterPos.y + dy];
		if (targetTile.Actor is null)
		{
			game.Log.Add($"The fist hits only the {targetTile.Terrain.Name}.");
		}
		else
		{
			var damage = caster.Stats.Strength + caster.Stats.Willpower;
			game.Log.Add($"The fist hits the {targetTile.Actor} ({damage} damage).");
			targetTile.Actor.TakeDamage(damage);
		}
	}
}
