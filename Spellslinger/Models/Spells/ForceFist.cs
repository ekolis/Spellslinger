using Spellslinger.Services;

namespace Spellslinger.Models.Spells;

public class ForceFist
	: Spell
{
	public ForceFist(IGame game)
		: base(game)
	{
	}

	public override string Name => "Force Fist";
	public override string Description => "Projects a short ranged ethereal fist made of pure kinetic energy.";
	public override string Details => "Damage scales with strength and willpower.";
	public override bool IsDirectional => true;
	public override int MPCost => 3;

	protected override void CastImpl(Actor caster, int dx, int dy)
	{
		var casterPos = Game.CurrentMap.LocateActor(caster);
		// TODO: deal with array index out of bounds if we try punching too far
		var targetTile = Game.CurrentMap.Tiles[casterPos.x + dx, casterPos.y + dy];
		if (targetTile.Actor is null)
		{
			Game.Log.Add($"The fist hits only the {targetTile.Terrain.Name}.");
		}
		else
		{
			var damage = caster.Stats.Strength + caster.Stats.Willpower;
			Game.Log.Add($"The fist hits the {targetTile.Actor} ({damage} damage).");
			targetTile.Actor.TakeDamage(damage);
		}
	}
}
