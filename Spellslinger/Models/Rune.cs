using Spellslinger.Models.SpellModifiers;
using Spellslinger.Models.Spells;

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
	/// The core spell that this rune can be used to cast, or null if it has no core spell.
	/// </summary>
	public Spell? Spell { get; set; }

	/// <summary>
	/// Modifier that is applied to spells containing this rune as a modifier rune.
	/// </summary>
	public SpellModifier Modifier { get; set; }

	public static Rune Force { get; } = new Rune()
	{
		Name = "Force",
		Spell = new ForceFist(),
		Modifier = new Force(),
	};
}
