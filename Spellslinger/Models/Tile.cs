using System.ComponentModel;
using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// A tile on the map.
/// </summary>
public class Tile
{
	public Tile(Map map, int x, int y, Terrain terrain)
	{
		Map = map;
		X = x;
		Y = y;
		Terrain = terrain;
	}

	public Map Map { get; }

	public int X { get; }

	public int Y { get; }

	/// <summary>
	/// The terrain of this tile.
	/// </summary>
	public Terrain Terrain { get; set; }

	/// <summary>
	/// Any treasures on this tile.
	/// </summary>
	public IList<Treasure> Treasures { get; } = [];

	private Actor? actor;

	/// <summary>
	/// The actor on this tile, if any.
	/// </summary>
	public Actor? Actor
	{
		get
		{
			return actor;
		}
		set
		{
			if (value != actor)
			{
				if (actor is not null)
				{
					actor.Tile = null;
				}
				actor = value;
				if (value is not null)
				{
					value.Tile = this;
				}
			}
		}
	}

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
