namespace Spellslinger.Models;

/// <summary>
/// Modes of input for the UI.
/// </summary>
public enum InputMode
{
	/// <summary>
	/// The player is viewing the title screen.
	/// </summary>
	Title,
	/// <summary>
	/// The player is selecting a character.
	/// </summary>
	CharacterSelection,
	/// <summary>
	/// The player is in the town.
	/// </summary>
	Town,
	/// <summary>
	/// The player is crafting spells.
	/// </summary>
	Spellcrafting,
	/// <summary>
	/// The player is shopping.
	/// </summary>
	Shopping,
	/// <summary>
	/// The player is training their stats.
	/// </summary>
	Training,
	/// <summary>
	/// The player is exploring the dungeon.
	/// </summary>
	Dungeon,
	/// <summary>
	/// The player is selecting a direction to cast a spell.
	/// </summary>
	SpellDirection,
	/// <summary>
	/// The player has won the game!
	/// </summary>
	Victory,
	/// <summary>
	/// The player triggered the secret bad ending by awakening the boss and leaving the dungeon without collecting the artifact!
	/// </summary>
	BadEnding,
}
