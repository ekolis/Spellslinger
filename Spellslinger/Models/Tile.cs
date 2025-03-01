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
	/// The actor on this tile, if any.
	/// </summary>
	public Actor? Actor { get; set; }

	/// <summary>
	/// The character used to represent the tile.
	/// </summary>
	public char Character => Actor?.Character ?? Terrain.Character;

	/// <summary>
	/// The foreground color used to represent the tile.
	/// </summary>
	public Color Color => Actor?.Color ?? Terrain.Color;
}
