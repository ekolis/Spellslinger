using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// A terrain that can be on a tile.
/// </summary>
/// <param name="Character">The character used to represent the terrain.</param>
/// <param name="Color">The foreground color used to represent the terrain.</param>
/// <param name="IsPassable">Is this terrain able to be moved through?</param>
public record Terrain(char Character, Color Color, bool IsPassable)
{
	public static Terrain Floor { get; } = new Terrain('.', Color.Gray, true);

	public static Terrain Wall { get; } = new Terrain('#', Color.White, false);

	public static Terrain Door { get; } = new Terrain('+', Color.Yellow, true);

	public static Terrain StairsUp { get; } = new Terrain('<', Color.Green, true);

	public static Terrain StairsDown { get; } = new Terrain('>', Color.Green, true);
}
