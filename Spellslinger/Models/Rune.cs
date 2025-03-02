namespace Spellslinger.Models;

/// <summary>
/// A rune that can be used to craft spells.
/// </summary>
public class Rune
{
	/// <summary>
	/// The name of this rune.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// The core spell that this rune can be used to cast.
	/// </summary>
	public Spell Spell { get; set; }

	/// <summary>
	/// Modifiers that are applied to spells containing this rune.
	/// </summary>
	public IList<SpellModifier> Modifiers { get; } = [];
}
