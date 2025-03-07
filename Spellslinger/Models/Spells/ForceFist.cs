using Spellslinger.Services;

namespace Spellslinger.Models.Spells;

public record ForceFist
	: Spell
{
	public ForceFist()
	{
		Stats = new SpellStats(
			Name: x => $"{x.Element} Fist",
			Description: x => $"Projects a short ranged ethereal fist made of pure {x.Element.Description}.",
			Details: x => "Damage scales with strength and willpower.",
			MPCost: 3,
			Element: Element.Force,
			Power: x => x.Strength + x.Willpower,
			Knockback: x => 0,
			Teleport: x => 0,
			Range: x => 1);
	}

	public override bool IsDirectional => true;

	protected override void CastImpl(IGame game, Actor caster, int dx, int dy)
	{
		CastBolt(game, caster, dx, dy);
	}
}
