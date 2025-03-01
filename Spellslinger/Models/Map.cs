namespace Spellslinger.Models;

/// <summary>
/// A map on which the game is played.
/// </summary>
public class Map
{
	public Map(int width, int height)
	{
		Width = width;
		Height = height;
		Tiles = new Tile[width, height];
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				Tiles[x, y] = new Tile
				{
					Character = '.'
				};
			}
		}
	}

	/// <summary>
	/// The tiles on the map.
	/// </summary>
	public Tile[,] Tiles { get; }

	public int Width { get; }

	public int Height { get; }
}
