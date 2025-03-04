namespace Spellslinger.Models;

/// <summary>
/// A modifier for a spell.
/// </summary>
/// <param name="Description">A brief description of the modifier's effects.</param>
/// <param name="Details">Additional details on the modifier's effects, such as formulas and stats used.</param>
/// <param name="Element">The element to change the spell to, or null to not affect its element.</param>
/// <param name="MPCost">The number of additional MP required to cast a spell with this modifier.</param>
/// <param name="Power">Modifier for the spell power. This is typically used for damage, healing, defense, etc.</param>
/// <param name="Knockback">The amount of knockback to add to the spell.</param>
/// <param name="Range">The amount of range to add to the spell.</param>
public record SpellModifier(string Description, string Details, Element? Element, int MPCost, Func<ActorStats, int> Power, Func<ActorStats, int> Knockback, Func<ActorStats, int> Range)
{
	public SpellStats Apply(SpellStats stats)
	{
		return stats with
		{
			Element = Element ?? stats.Element,
			MPCost = stats.MPCost + MPCost,
			Power = (actorStats) => stats.Power(actorStats) + Power(actorStats)
		};
	}

	public static SpellModifier Force { get; } = new(
		Description: "Increases the power of a spell.",
		Details: "Power increase scales with strength.",
		Element: null,
		MPCost: 3,
		Power: x => x.Strength,
		Knockback: x => 0,
		Range : x => 0
	);

	public static SpellModifier Fire { get; } = new(
		Description: "Changes the element of a spell to fire and slightly increases power.",
		Details: "Power increase scales with willpower.",
		Element: Element.Fire,
		MPCost: 2,
		Power: x => x.Willpower / 2,
		Knockback: x => 0,
		Range: x => 0
	);

	public static SpellModifier Ice { get; } = new(
		Description: "Changes the element of a spell to ice and slightly increases power.",
		Details: "Power increase scales with toughness.",
		Element: Element.Ice,
		MPCost: 2,
		Power: x => x.Toughness / 2,
		Knockback: x => 0,
		Range: x => 0
	);

	public static SpellModifier Air { get; } = new(
		Description: "Changes the element of a spell to air and adds knockback.",
		Details: "Knockback scales with willpower.",
		Element: Element.Air,
		MPCost: 2,
		Power: x => 0,
		Knockback: x => 1 + x.Willpower / 5,
		Range: x => 0
	);

	public static SpellModifier Earth { get; } = new(
		Description: "Changes the element of a spell to earth and adds knockback.",
		Details: "Knockback scales with toughness.",
		Element: Element.Earth,
		MPCost: 2,
		Power: x => 0,
		Knockback: x => 1 + x.Toughness / 5,
		Range: x => 0
	);
}
