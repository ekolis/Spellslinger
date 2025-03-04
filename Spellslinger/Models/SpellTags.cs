namespace Spellslinger.Models;

[Flags]
public enum SpellTags
{
	/// <summary>
	/// No tags are applied.
	/// </summary>
	None = 0,
	/// <summary>
	/// Spell only affects stamina, not health.
	/// </summary>
	StaminaOnly = 1,
}
