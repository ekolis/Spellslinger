using Spellslinger.Services;

namespace Spellslinger.Models.Spells;

public record Teleport
	: Spell
{
	public Teleport()
	{
		Stats = new SpellStats(
			Name: x => $"Teleport",
			Description: x => $"Teleports the caster a moderate distance.",
			Details: x => "Distance scales with Willpower and is affected by Power, Range, Knockback, and Teleport modifiers.",
			MPCost: 6,
			Element: null,
			Power: x => 0,
			Knockback: x => 0,
			Teleport: x => 5 + x.Willpower,
			Range: x => 0);
	}

	public override bool IsDirectional => false;

	protected override void CastImpl(IGame game, Actor caster, int dx, int dy)
	{
		var maxDistance = Stats.Power(caster.Stats) + Stats.Knockback(caster.Stats) + Stats.Teleport(caster.Stats) + Stats.Range(caster.Stats);
		TeleportActor(game, caster, maxDistance);
	}
}
