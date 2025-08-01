﻿using Spellslinger.Services;

namespace Spellslinger.Models.Spells;

public record EarthGuard
	: Spell
{
	public EarthGuard()
	{
		Stats = new SpellStats(
			Name: x => $"{x.Element} Guard",
			Description: x => $"Heals the caster and blasts adjacent enemies with {x.Element.Description} that knock them back.",
			Details: x => "Healing, damage, and knockback scale with toughness.",
			MPCost: 6,
			Element: Element.Earth,
			Power: x => x.Toughness,
			Knockback: x => x.Toughness / 2,
			Teleport: x => 0,
			Range: x => 1);
	}

	public override bool IsDirectional => false;

	protected override void CastImpl(IGame game, Actor caster, int dx, int dy)
	{
		var casterPos = game.CurrentMap.LocateActor(caster);

		var power = Stats.Power(caster.Stats);

		// heal the caster
		game.Log.Add($"The {caster} is healed for {power} HP.");
		caster.HP.Restore(power);

		// find all adjacent tiles and hit them
		var range = Stats.Range(caster.Stats);
		for (var x = casterPos.x - range; x <= casterPos.x + range; x++)
		{
			for (var y = casterPos.y - range; y <= casterPos.y + range; y++)
			{
				var distance = Math.Abs(x - casterPos.x) + Math.Abs(y - casterPos.y);
				if (distance > 0 && distance <= range)
				{
					HitTile(game, caster, Stats.Tags, power, Stats.Knockback(caster.Stats), Stats.Teleport(caster.Stats), x, y, dx, dy);
				}
			}
		}
	}
}
