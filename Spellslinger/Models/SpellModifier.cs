namespace Spellslinger.Models;

/// <summary>
/// A modifier for a spell.
/// </summary>
/// <param name="SpellName">A function to apply to change the spell name.</param>
/// <param name="Description">A brief description of the modifier's effects.</param>
/// <param name="Details">Additional details on the modifier's effects, such as formulas and stats used.</param>
/// <param name="Element">The element to change the spell to, or null to not affect its element.</param>
/// <param name="MPCost">The number of additional MP required to cast a spell with this modifier.</param>
/// <param name="Power">Modifier for the spell power. This is typically used for damage, healing, defense, etc.</param>
/// <param name="Knockback">The amount of knockback to add to the spell.</param>
/// <param name="Range">The amount of range to add to the spell.</param>
/// <param name="Tags">Any special tags to apply to the spell.</param>
public record SpellModifier(Func<string, string> SpellName, string Description, string Details, Element? Element, int MPCost, Func<ActorStats, int> Power, Func<ActorStats, int> Knockback, Func<ActorStats, int> Range, SpellTags Tags = SpellTags.None)
{
	public SpellStats Apply(SpellStats stats)
	{
		return stats with
		{
			Name = x => SpellName(stats.Name(x)),
			Element = Element ?? stats.Element,
			MPCost = stats.MPCost + MPCost,
			Power = (actorStats) => stats.Power(actorStats) + Power(actorStats),
			Knockback = stats.Knockback + Knockback,
			Range = stats.Range + Range,
			Tags = stats.Tags | Tags
		};
	}

	public static SpellModifier Force { get; } = new(
		SpellName: x => "Power " + x,
		Description: "Increases the power of a spell.",
		Details: "Power scales with strength.",
		Element: null,
		MPCost: 3,
		Power: x => x.Strength,
		Knockback: x => 0,
		Range : x => 0
	);

	public static SpellModifier Fire { get; } = new(
		SpellName: x => x,
		Description: "Changes the element of a spell to fire and slightly increases power.",
		Details: "Power scales with willpower.",
		Element: Element.Fire,
		MPCost: 2,
		Power: x => x.Willpower / 2,
		Knockback: x => 0,
		Range: x => 0
	);

	public static SpellModifier Ice { get; } = new(
		SpellName: x => x,
		Description: "Changes the element of a spell to ice and slightly increases power.",
		Details: "Power scales with toughness.",
		Element: Element.Ice,
		MPCost: 2,
		Power: x => x.Toughness / 2,
		Knockback: x => 0,
		Range: x => 0
	);

	public static SpellModifier Air { get; } = new(
		SpellName: x => x,
		Description: "Changes the element of a spell to air and adds knockback.",
		Details: "Knockback scales with willpower.",
		Element: Element.Air,
		MPCost: 2,
		Power: x => 0,
		Knockback: x => 1 + x.Willpower / 5,
		Range: x => 0
	);

	public static SpellModifier Earth { get; } = new(
		SpellName: x => x,
		Description: "Changes the element of a spell to earth and adds knockback.",
		Details: "Knockback scales with toughness.",
		Element: Element.Earth,
		MPCost: 2,
		Power: x => 0,
		Knockback: x => 1 + x.Toughness / 5,
		Range: x => 0
	);

	public static SpellModifier Kale { get; } = new(
		SpellName: x => "Ka" + x[0].ToString().ToLower() + x.Substring(1) + "le",
		Description: "Imbues a spell with the mystical vegan power of kale, making it more powerful but affecting stamina only (not health), and adding knockback.",
		Details: "Power scales with willpower, knockback scales with strength.",
		Element: null,
		MPCost: 4,
		Power: x => x.Willpower,
		Knockback: x => x.Strength / 3,
		Range: x => 0,
		Tags: SpellTags.StaminaOnly
	);

	public static SpellModifier Extend { get; } = new(
		SpellName: x => "Extended " + x,
		Description: "Extends the range of a spell.",
		Details: "Range scales with willpower.",
		Element: null,
		MPCost: 2,
		Power: x => 0,
		Knockback: x => 0,
		Range: x => 1 + x.Willpower / 5
	);

	public static SpellModifier Focus { get; } = new(
		SpellName: x => "Focused " + x,
		Description: "Shortens the range of a spell, but greatly increases its power.",
		Details: "Power scales with willpower and memory.",
		Element: null,
		MPCost: 4,
		Power: x => x.Memory + x.Willpower,
		Knockback: x => 0,
		Range: x => -1
	);
}
