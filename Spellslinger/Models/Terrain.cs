using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// A terrain that can be on a tile.
/// </summary>
/// <param name="Character">The character used to represent the terrain.</param>
/// <param name="Color">The foreground color used to represent the terrain.</param>
public record Terrain(char Character, Color Color)
{
	public static Terrain Floor { get; } = new Terrain('.', Color.Gray);

	public static Terrain Wall { get; } = new Terrain('#', Color.White);

	public static Terrain Door { get; } = new Terrain('+', Color.Yellow);

	public static Terrain StairsUp { get; } = new Terrain('<', Color.Green);

	public static Terrain StairsDown { get; } = new Terrain('>', Color.Green);
}
