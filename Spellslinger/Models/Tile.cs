using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// A tile on the map.
/// </summary>
public class Tile
{
	/// <summary>
	/// The terrain of this tile.
	/// </summary>
	public required Terrain Terrain { get; set; }

	/// <summary>
	/// The character used to represent the tile.
	/// </summary>
	public char Character => Terrain.Character;

	/// <summary>
	/// The foreground color used to represent the tile.
	/// </summary>
	public Color Color => Terrain.Color;
}
