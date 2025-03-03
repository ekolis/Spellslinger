using Spellslinger.Services;

namespace Spellslinger.Models;

/// <summary>
/// Things that an actor knows about.
/// </summary>
public class Knowledge
{
	/// <summary>
	/// The runes that this actor can use to cast spells.
	/// </summary>
	public IList<Rune> Runes { get; } = [];

	/// <summary>
	/// The spells that this actor knows.
	/// </summary>
	public IList<Spell> Spells { get; } = [];

	/// <summary>
	/// Any spells that are prepared to cast after the actor makes a melee attack.
	/// </summary>
	public IList<Spell> MeleeSpells { get; } = [];

	/// <summary>
	/// Any spells that are prepared to cast at will.
	/// </summary>
	public IList<Spell> GeneralSpells { get; } = [];
}
