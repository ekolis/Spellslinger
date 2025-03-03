namespace Spellslinger.Models;

/// <summary>
/// A modifier for a spell.
/// </summary>
public abstract class SpellModifier
{
	/// <summary>
	/// A brief description of the modifier's effects.
	/// </summary>
	public abstract string Description { get; }

	/// <summary>
	/// Additional details on the modifier's effects, such as formulas and stats used.
	/// </summary>
	public abstract string Details { get; }

	/// <summary>
	/// The number of additional MP required to cast a spell with this modifier.
	/// </summary>
	public abstract int MPCost { get; }
}
