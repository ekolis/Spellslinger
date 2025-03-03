namespace Spellslinger.Models;

/// <summary>
/// Modifiable stats that apply to spells.
/// </summary>
/// <param name="Name">The name of the spell.</param>
/// <param name="Element">The element of the spell (e.g fire or force).</param>
/// <param name="Description">A brief description of the spell's effects.</param>
/// <param name="Details">Additional details on the spell's effects, such as formulas and stats used.</param>
/// <param name="MPCost">The number of MP required to cast the spell.</param>
/// <param name="Knockback">Tries to knock the enemy back (chance rolled against target toughness).</param>
/// <param name="Power">The power of the spell. This typically corresponds to something like damage, healing, defense...</param>
/// <param name="Range">The range of the spell.</param>
public record SpellStats(Func<SpellStats, string> Name, Element Element, Func<SpellStats, string> Description, Func<SpellStats, string> Details, int MPCost, Func<ActorStats, int> Power, Func<ActorStats, int> Knockback, Func<ActorStats, int> Range);
