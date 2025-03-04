using Spellslinger.Models.Spells;

namespace Spellslinger.Models;

/// <summary>
/// A rune that can be used to craft spells.
/// </summary>
/// <param name="Name">The name of this rune.</param>
/// <param name="Spell">The core spell that this rune can be used to cast, or null if it has no core spell.</param>
/// <param name="Modifier">Modifier that is applied to spells containing this rune as a modifier rune.</param>
/// <param name="Cost">The gold cost to buy this rune.</param>
public record Rune(string Name, Spell? Spell, SpellModifier Modifier, int Cost)
{
	public static Rune Force { get; } = new("Force", new ForceFist(), SpellModifier.Force, 50);
	public static Rune Fire { get; } = new("Fire", new FireWave(), SpellModifier.Fire, 50);
	public static Rune Ice { get; } = new("Ice", new IceStorm(), SpellModifier.Ice, 50);
	public static Rune Air { get; } = new("Air", new AirVortex(), SpellModifier.Air, 50);
	public static Rune Earth { get; } = new("Earth", new EarthGuard(), SpellModifier.Earth, 50);
	public static Rune Vegan { get; } = new("Vegan", null, SpellModifier.Kale, 25);
	public static Rune Extend { get; } = new("Extend", null, SpellModifier.Extend, 25);
	public static Rune Focus { get; } = new("Focus", null, SpellModifier.Focus, 25);

	public static IEnumerable<Rune> All { get; } = [Force, Fire, Ice, Air, Earth, Vegan, Extend, Focus];

	public override string ToString()
	{
		return Name;
	}
}
