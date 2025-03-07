using System.Drawing;
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
			Teleport: x => 0,
			Range: x => x.Memory);
	}

	public override bool IsDirectional => true;

	protected override void CastImpl(IGame game, Actor caster, int dx, int dy)
	{
		CastBolt(game, caster, dx, dy);
	}
}
