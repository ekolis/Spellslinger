using Spellslinger.Services;

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
					Terrain = Terrain.Floor
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

	/// <summary>
	/// Finds the first actor (if any) matching a predicate.
	/// </summary>
	/// <param name="predicate"></param>
	/// <returns></returns>
	public Actor? FindActor(Func<Actor, bool> predicate)
	{
		return Tiles.Cast<Tile>().Select(q => q.Actor).Where(q => q is not null).FirstOrDefault(predicate);
	}

	/// <summary>
	/// Determines the location of an actor.
	/// </summary>
	/// <param name="actor"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public (int x, int y) LocateActor(Actor actor)
	{
		for (var x = 0; x < Width; x++)
		{
			for (var y = 0; y < Height; y++)
			{
				if (Tiles[x, y].Actor == actor)
				{
					return (x, y);
				}
			}
		}
		throw new InvalidOperationException($"Actor {actor} was not found on this map.");
	}

	/// <summary>
	/// Moves an actor.
	/// </summary>
	/// <param name="actor"></param>
	/// <param name="dx"></param>
	/// <param name="dy"></param>
	/// <returns>true if a turn was taken, false if not</returns>
	public bool MoveActor(Actor actor, int dx, int dy)
	{
		if (dx == 0 && dy == 0)
		{
			// pass a turn, don't attack yourself
			return true;
		}

		// verify movement location
		var (x, y) = LocateActor(actor);
		var newX = x + dx;
		var newY = y + dy;
		if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
		{
			// can't move off the map
			return false;
		}
		if (Tiles[newX, newY].Actor is not null)
		{
			// TODO: combat
			return false;
		}

		// move the actor
		Tiles[x, y].Actor = null;
		Tiles[newX, newY].Actor = actor;

		// yes, we did take a turn
		return true;
	}
}
