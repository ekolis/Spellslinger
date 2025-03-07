using Spellslinger.Models.Spells;

namespace Spellslinger.Models;

/// <summary>
/// A rune that can be used to craft spells.
/// </summary>
/// <param name="Name">The name of this rune.</param>
/// <param name="Spell">The core spell that this rune can be used to cast, or null if it has no core spell.</param>
/// <param name="Modifier">Modifier that is applied to spells containing this rune as a modifier rune.</param>
/// <param name="Cost">The gold cost to buy this rune. Will be multiplied by <see cref="IGame.RuneCostFactor"/>.</param>
public record Rune(string Name, Spell? Spell, SpellModifier Modifier, int Cost)
{
	public static Rune Force { get; } = new("Force", new ForceFist(), SpellModifier.Force, 2);
	public static Rune Fire { get; } = new("Fire", new FireWave(), SpellModifier.Fire, 2);
	public static Rune Ice { get; } = new("Ice", new IceStorm(), SpellModifier.Ice, 2);
	public static Rune Air { get; } = new("Air", new AirVortex(), SpellModifier.Air, 2);
	public static Rune Earth { get; } = new("Earth", new EarthGuard(), SpellModifier.Earth, 2);
	public static Rune Vegan { get; } = new("Vegan", null, SpellModifier.Kale, 1);
	public static Rune Extend { get; } = new("Extend", null, SpellModifier.Extend, 1);
	public static Rune Focus { get; } = new("Focus", null, SpellModifier.Focus, 1);
	public static Rune Warp { get; } = new("Warp", new Teleport(), SpellModifier.Warp, 2);
	public static Rune Ascend { get; } = new("Ascend", new Ascend(), SpellModifier.Air, 3);
	public static Rune Descend { get; } = new("Descend", new Descend(), SpellModifier.Earth, 3);

	public static IEnumerable<Rune> All { get; } = [Force, Fire, Ice, Air, Earth, Vegan, Extend, Focus];

	public override string ToString()
	{
		return Name;
	}
}
