using Spellslinger.Models;

namespace Spellslinger.Services;

public class MapGenerator
	: IMapGenerator
{
	public Map Generate(IGame game, int width, int height, int rooms, int extraDoors)
	{
		var map = new Map(width, height);

		// put a wall around the edge
		BuildHorizontalWall(map, 0);
		BuildHorizontalWall(map, height - 1);
		BuildVerticalWall(map, 0);
		BuildVerticalWall(map, width - 1);


		// use binary space partitioning to split the map into rooms
		// TODO: why are we still getting walls going into doors?
		// maybe just clean up the space around all doors, if three sides of a door are walls, delete the one opposite a floor?
		var rng = new Random();
		List<int> verticalWallPositions = [];
		List<int> horizontalWallPositions = [];
		for (var i = 0; i < rooms - 1; i++)
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

		// place some random doors to better connect the rooms
		for (var i = 0; i < extraDoors; i++)
		{
			var doorCandidates = new List<(int x, int y)>();
			for (var x = 1; x < width - 1; x++)
			{
				for (var y = 1; y < height - 1; y++)
				{
					// place a door in a wall with floors on either side
					if (map.Tiles[x, y].Terrain == Terrain.Wall
						&& ((map.Tiles[x - 1, y].Terrain == Terrain.Floor && map.Tiles[x + 1, y].Terrain == Terrain.Floor)
						|| (map.Tiles[x, y - 1].Terrain == Terrain.Floor && map.Tiles[x, y + 1].Terrain == Terrain.Floor)))
					{
						doorCandidates.Add((x, y));
					}
				}
			}
			if (!doorCandidates.Any())
			{
				break;
			}
			var doorPos = doorCandidates[rng.Next(doorCandidates.Count)];
			map.Tiles[doorPos.x, doorPos.y].Terrain = Terrain.Door;
		}

		// place up and down stairs
		var stairCandidates = new List<(int x, int y)>();
		for (var x = 1; x < width - 1; x++)
		{
			for (var y = 1; y < height - 1; y++)
			{
				// place stairs on a floor tile
				if (map.Tiles[x, y].Terrain == Terrain.Floor)
				{
					stairCandidates.Add((x, y));
				}
			}
		}
		var upStairPos = stairCandidates[rng.Next(stairCandidates.Count)];
		map.Tiles[upStairPos.x, upStairPos.y].Terrain = Terrain.StairsUp;
		stairCandidates.Remove(upStairPos);
		var downStairPos = stairCandidates[rng.Next(stairCandidates.Count)];
		map.Tiles[downStairPos.x, downStairPos.y].Terrain = Terrain.StairsDown;

		// place the player on the up stairs
		map.Tiles[upStairPos.x, upStairPos.y].Actor = new Actor(ActorType.Player, game);

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
		map.Tiles[xWallPos, yDoorPos].Terrain = Terrain.Door;
		for (var y = yDoorPos + 1; y < map.Height && map.Tiles[xWallPos, y].Terrain == Terrain.Floor; y++)
		{
			map.Tiles[xWallPos, y].Terrain = Terrain.Wall;
		}
		for (var y = yDoorPos - 1; y >= 0 && map.Tiles[xWallPos, y].Terrain == Terrain.Floor; y--)
		{
			map.Tiles[xWallPos, y].Terrain = Terrain.Wall;
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
		map.Tiles[xDoorPos, yWallPos].Terrain = Terrain.Door;
		for (var x = xDoorPos + 1; x < map.Width && map.Tiles[x, yWallPos].Terrain == Terrain.Floor; x++)
		{
			map.Tiles[x, yWallPos].Terrain = Terrain.Wall;
		}
		for (var x = xDoorPos - 1; x < map.Width && map.Tiles[x, yWallPos].Terrain == Terrain.Floor; x--)
		{
			map.Tiles[x, yWallPos].Terrain = Terrain.Wall;
		}
	}
}
