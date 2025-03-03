namespace Spellslinger.Models;

/// <summary>
/// Modes of input for the UI.
/// </summary>
public enum InputMode
{
	/// <summary>
	/// The player is exploring the dungeon.
	/// </summary>
	Exploration,
	/// <summary>
	/// The player is selecting a direction to cast a spell.
	/// </summary>
	SpellDirection,
	/// <summary>
	/// The player is crafting spells.
	/// </summary>
	Spellcrafting,
}
