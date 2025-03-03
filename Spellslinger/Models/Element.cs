using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// An elemental effect of a spell.
/// </summary>
/// <param name="Name"></param>
/// <param name="Description"></param>
/// <param name="Color"></param>
public record Element(string Name, string Description, string Verb, Color Color)
{
	public override string ToString()
	{
		return Name;
	}

	public static Element Force { get; } = new("Force", "kinetic energy", "impacts", Color.White);
	public static Element Fire { get; } = new("Fire", "searing flames", "burn", Color.Red);
	public static Element Ice { get; } = new("Ice", "frigid icicles", "freeze", Color.Blue);
	public static Element Air { get; } = new("Air", "gusts of wind", "buffet", Color.Green);
	public static Element Earth { get; } = new("Earth", "hard stones", "impact", Color.Brown);
}
