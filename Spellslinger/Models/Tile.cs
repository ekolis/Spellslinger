using System.ComponentModel;
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
	/// Any treasures on this tile.
	/// </summary>
	public IList<Treasure> Treasures { get; } = [];

	/// <summary>
	/// The actor on this tile, if any.
	/// </summary>
	public Actor? Actor { get; set; }

	/// <summary>
	/// The effect on this tile, if any.
	/// </summary>
	public Effect? Effect { get; set; }

	/// <summary>
	/// The character used to represent the tile.
	/// </summary>
	public char Character =>
		Effect?.Character
		?? Actor?.Character
		?? Treasures.OrderBy(q => q.Priority).FirstOrDefault()?.Character
		?? Terrain.Character;

	/// <summary>
	/// The foreground color used to represent the tile.
	/// </summary>
	public Color Color =>
		Effect?.Color
		?? Actor?.Color
		?? Treasures.OrderBy(q => q.Priority).FirstOrDefault()?.Color
		?? Terrain.Color;

	/// <summary>
	/// The background color used to represent this tile.
	/// </summary>
	public Color BackgroundColor =>
		Effect?.BackgroundColor
		//?? Terrain.BackgroundColor
		?? Color.Black;
}
