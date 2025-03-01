using Spellslinger.Models;

namespace Spellslinger.Services;

public class MapGenerator
	: IMapGenerator
{
	public Map Generate(int width, int height)
	{
		var map = new Map(width, height);

		// put a wall around the edge
		BuildHorizontalWall(map, 0);
		BuildHorizontalWall(map, height - 1);
		BuildVerticalWall(map, 0);
		BuildVerticalWall(map, width - 1);
		

		// use binary space partitioning to split the map into rooms
		var rng = new Random();
		List<int> verticalWallPositions = [];
		List<int> horizontalWallPositions = [];
		for (var i = 0; i < 10; i++)
		{
			// decide where to place a door
			// don't try to place a door in an existing wall, we're going to build a new wall
			// (a great wall, like no wall the world has ever seen, and we'll make Mexico pay for it)
			// give up after 10 tries
			// also don't place walls next to other walls, that blocks off the map
			// and don't have walls going through doors
			int x, y;
			int tries = 0;
			do
			{
				x = rng.Next(2, width - 2);
				y = rng.Next(2, height - 2);
				tries++;
			} while (
				(map.Tiles[x, y].Terrain == Terrain.Wall
				|| map.Tiles[x - 1, y].Terrain == Terrain.Wall
				|| map.Tiles[x + 1, y].Terrain == Terrain.Wall
				|| map.Tiles[x, y - 1].Terrain == Terrain.Wall
				|| map.Tiles[x, y + 1].Terrain == Terrain.Wall
				|| verticalWallPositions.Contains(x)
				|| horizontalWallPositions.Contains(y))
				&& tries < 10);

			if (i % 2 == 0)
			{
				BuildVerticalWall(map, x, y);
				verticalWallPositions.Add(x);
			}
			else
			{
				BuildHorizontalWall(map, y, x);
				horizontalWallPositions.Add(y);
			}
		}

		return map;
	}

	private void BuildVerticalWall(Map map, int xWallPos)
	{
		for (var y = 0; y < map.Height; y++)
		{
			map.Tiles[xWallPos, y].Terrain = Terrain.Wall;
		}
	}

	private void BuildVerticalWall(Map map, int xWallPos, int yDoorPos)
	{
		for (var y = yDoorPos + 1; y < map.Height && map.Tiles[xWallPos, y].Terrain == Terrain.Floor; y++)
		{
			if (y == yDoorPos)
			{
				map.Tiles[xWallPos, y].Terrain = Terrain.Door;
			}
			else
			{
				map.Tiles[xWallPos, y].Terrain = Terrain.Wall;
			}
		}
		for (var y = yDoorPos - 1; y >= 0 && map.Tiles[xWallPos, y].Terrain == Terrain.Floor; y--)
		{
			if (y == yDoorPos)
			{
				map.Tiles[xWallPos, y].Terrain = Terrain.Door;
			}
			else
			{
				map.Tiles[xWallPos, y].Terrain = Terrain.Wall;
			}
		}
	}

	private void BuildHorizontalWall(Map map, int yWallPos)
	{
		for (var x = 0; x < map.Width; x++)
		{
			map.Tiles[x, yWallPos].Terrain = Terrain.Wall;
		}
	}

	private void BuildHorizontalWall(Map map, int yWallPos, int xDoorPos)
	{
		for (var x = xDoorPos + 1; x < map.Width && map.Tiles[x, yWallPos].Terrain == Terrain.Floor; x++)
		{
			if (x == xDoorPos)
			{
				map.Tiles[x, yWallPos].Terrain = Terrain.Door;
			}
			else
			{
				map.Tiles[x, yWallPos].Terrain = Terrain.Wall;
			}
		}
		for (var x = xDoorPos - 1; x < map.Width && map.Tiles[x, yWallPos].Terrain == Terrain.Floor; x--)
		{
			if (x == xDoorPos)
			{
				map.Tiles[x, yWallPos].Terrain = Terrain.Door;
			}
			else
			{
				map.Tiles[x, yWallPos].Terrain = Terrain.Wall;
			}
		}
	}
}
