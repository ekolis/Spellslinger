using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// A terrain that can be on a tile.
/// </summary>
/// <param name="Name">The name of the terrain.</param>
/// <param name="Character">The character used to represent the terrain.</param>
/// <param name="Color">The foreground color used to represent the terrain.</param>
/// <param name="IsPassable">Is this terrain able to be moved through?</param>
public record Terrain(string Name, char Character, Color Color, bool IsPassable)
{
	public static Terrain Floor { get; } = new Terrain("floor", '.', Color.Gray, true);

	public static Terrain Wall { get; } = new Terrain("wall", '#', Color.White, false);

	public static Terrain Door { get; } = new Terrain("door", '+', Color.Yellow, true);

	public static Terrain StairsUp { get; } = new Terrain("stairs going up", '<', Color.Green, true);

	public static Terrain StairsDown { get; } = new Terrain("stairs going down", '>', Color.Green, true);
}
