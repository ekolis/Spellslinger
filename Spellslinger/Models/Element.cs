using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// An elemental effect of a spell.
/// </summary>
/// <param name="Name"></param>
/// <param name="Description"></param>
/// <param name="Color"></param>
public record Element(string Name, string Description, Color Color)
{
	public override string ToString()
	{
		return Name;
	}

	public static Element Force { get; } = new("Force", "kinetic energy", Color.White);
	public static Element Fire { get; } = new("Fire", "searing flames", Color.Red);
}
