using Spellslinger.Services;

namespace Spellslinger.Models.Spells;

public record IceStorm
	: Spell
{
	public IceStorm()
	{
		Stats = new SpellStats(
			Name: x => $"{x.Element} Storm",
			Description: x => $"Blasts nearby enemies in all directions with {x.Element.Description}. Penetrates walls.",
			Details: x => "Damage scales with willpower, range with toughness.",
			MPCost: 6,
			Element: Element.Ice,
			Power: x => x.Willpower * 2,
			Knockback: x => 0,
			Range: x => x.Toughness / 2);
	}

	public override bool IsDirectional => false;

	protected override void CastImpl(IGame game, Actor caster, int dx, int dy)
	{
		var casterPos = game.CurrentMap.LocateActor(caster);

		// find all tiles within the specified radius and hit them
		var range = Stats.Range(caster.Stats);
		for (var x = casterPos.x - range; x <= casterPos.x + range; x++)
		{
			for (var y = casterPos.y - range; y <= casterPos.y + range; y++)
			{
				var distance = Math.Abs(x - casterPos.x) + Math.Abs(y - casterPos.y);
				if (distance > 0 && distance <= range)
				{
					HitTile(game, Stats.Power(caster.Stats), Stats.Knockback(caster.Stats), x, y, dx, dy);
				}
			}
		}
	}
}
