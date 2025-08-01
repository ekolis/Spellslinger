﻿using System;
using Spellslinger.Models;
using Spellslinger.Models.Spells;
using Spellslinger.Models.Treasures;

namespace Spellslinger.Services;

public class MapGenerator
	: IMapGenerator
{
	public const int MaxDepth = 8;

	public Map Generate(IGame game, int depth, bool ascending)
	{
		// calculate map stats based on depth
		// TODO: make shallower levels smaller and deeper levels bigger, once that can be handled nicely in the UI
		int width = 40;
		int height = 20;
		int rooms = 16 - depth;
		int extraDoors = 30 - depth * 3;
		int enemies = 8 + depth;
		bool includeDownStairs = depth < MaxDepth;

		var map = new Map(game, depth, width, height);

		// put a wall around the edge
		BuildHorizontalWall(map, 0);
		BuildHorizontalWall(map, height - 1);
		BuildVerticalWall(map, 0);
		BuildVerticalWall(map, width - 1);


		// use binary space partitioning to split the map into rooms
		// TODO: why are we still getting walls going into doors?
		// maybe just clean up the space around all doors, if three sides of a door are walls, delete the one opposite a floor?
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
				x = game.Rng.Next(2, width - 2);
				y = game.Rng.Next(2, height - 2);
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
			var doorPos = doorCandidates[game.Rng.Next(doorCandidates.Count)];
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
		var upStairPos = stairCandidates[game.Rng.Next(stairCandidates.Count)];
		map.Tiles[upStairPos.x, upStairPos.y].Terrain = Terrain.StairsUp;
		stairCandidates.Remove(upStairPos);
		var downStairPos = stairCandidates[game.Rng.Next(stairCandidates.Count)];
		if (includeDownStairs)
		{
			map.Tiles[downStairPos.x, downStairPos.y].Terrain = Terrain.StairsDown;
		}

		// delete walls that are blocking doors
		for (var x = 1; x < width - 1; x++)
		{
			for (var y = 1; y < height - 1; y++)
			{
				if (map.Tiles[x, y].Terrain == Terrain.Door)
				{
					if (map.Tiles[x + 1, y].Terrain == Terrain.Wall && map.Tiles[x - 1, y].Terrain == Terrain.Wall)
					{
						// walls to east and west, delete any walls to north or south
						if (map.Tiles[x, y + 1].Terrain == Terrain.Wall)
						{
							map.Tiles[x, y + 1].Terrain = Terrain.Floor;
						}
						if (map.Tiles[x, y - 1].Terrain == Terrain.Wall)
						{
							map.Tiles[x, y - 1].Terrain = Terrain.Floor;
						}
					} else if (map.Tiles[x, y + 1].Terrain == Terrain.Wall && map.Tiles[x, y - 1].Terrain == Terrain.Wall)
					{
						// walls to north and south, delete any walls to east or west
						if (map.Tiles[x + 1, y].Terrain == Terrain.Wall)
						{
							map.Tiles[x + 1, y].Terrain = Terrain.Floor;
						}
						if (map.Tiles[x - 1, y].Terrain == Terrain.Wall)
						{
							map.Tiles[x - 1, y].Terrain = Terrain.Floor;
						}
					}
				}
			}
		}

		// place the player on the stairs
		if (ascending)
		{
			map.Tiles[downStairPos.x, downStairPos.y].Actor = game.Player;
		}
		else
		{
			map.Tiles[upStairPos.x, upStairPos.y].Actor = game.Player;
		}

		// place some random enemies
		for (var i = 0; i < enemies; i++)
		{
			// pick a random enemy, weighted toward enemies that are near in depth to the current dungeon level
			// but no more than 4 levels out of depth
			var enemyType = ActorType.Enemies
				.Where(q => Math.Abs(q.Depth - depth) <= 4)
				.PickWeighted(q => 1.0 / Math.Pow(Math.Abs(q.Depth - depth) + 1, 3), game.Rng);
			var enemy = new Actor(enemyType, game);
			List<(int x, int y)> enemyCandidates = [];
			for (var x = 1; x < width - 1; x++)
			{
				for (var y = 1; y < height - 1; y++)
				{
					// place enemies on an unoccupied floor tile
					if (map.Tiles[x, y].Terrain == Terrain.Floor && map.Tiles[x, y].Actor is null)
					{
						enemyCandidates.Add((x, y));
					}
				}
			}
			if (!enemyCandidates.Any())
			{
				break;
			}
			var enemyCandidate = enemyCandidates[game.Rng.Next(enemyCandidates.Count)];
			map.Tiles[enemyCandidate.x, enemyCandidate.y].Actor = enemy;
		}

		if (depth == MaxDepth && !game.IsArtifactCollected)
		{
			// place the artifact
			var artifact = new Artifact(game);
			List<(int x, int y)> artifactCandidates = [];
			for (var x = 1; x < width - 1; x++)
			{
				for (var y = 1; y < height - 1; y++)
				{
					// place artifact on an unoccupied floor tile
					if (map.Tiles[x, y].Terrain == Terrain.Floor && map.Tiles[x, y].Actor is null)
					{
						artifactCandidates.Add((x, y));
					}
				}
			}
			// TODO: deal with scenario where there's no place to put the artifact
			var artifactCandidate = artifactCandidates.PickWeighted(q => 1, game.Rng);
			map.Tiles[artifactCandidate.x, artifactCandidate.y].Treasures.Add(artifact);
			artifact.OnSpawn(map, artifactCandidate.x, artifactCandidate.y);
		}

		// set a recall point
		game.MaxDepthReached = Math.Max(game.MaxDepthReached, depth);

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
