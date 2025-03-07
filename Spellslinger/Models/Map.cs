using Spellslinger.Services;

namespace Spellslinger.Models;

/// <summary>
/// A map on which the game is played.
/// </summary>
public class Map
{
	public Map(IGame game, int depth, int width, int height)
	{
		Game = game;
		Depth = depth;
		Width = width;
		Height = height;
		Tiles = new Tile[width, height];
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				Tiles[x, y] = new Tile(this, x, y, Terrain.Floor);
			}
		}
	}

	private IGame Game { get; }

	/// <summary>
	/// The tiles on the map.
	/// </summary>
	public Tile[,] Tiles { get; }

	/// <summary>
	/// How deep is this map in the dungeon?
	/// </summary>
	public int Depth { get; }

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
		if (actor.Tile is null || actor.Tile.Map != this)
		{
			throw new InvalidOperationException($"Actor {actor} was not found on this map.");
		}
		else
		{
			return (actor.Tile.X, actor.Tile.Y);
		}
	}

	/// <summary>
	/// Moves an actor.
	/// </summary>
	/// <param name="actor"></param>
	/// <param name="dx"></param>
	/// <param name="dy"></param>
	/// <returns>true if a turn was taken, false if not</returns>
	public async Task<bool> MoveActor(Actor actor, int dx, int dy)
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
			// see if who's there is an enemy
			if (actor.IsPlayerControlled != Tiles[newX, newY].Actor.IsPlayerControlled)
			{
				await actor.Attack(Tiles[newX, newY].Actor);
				actor.ScheduleNextTurn();
				return true;
			}
			else
			{
				// can't walk through other actors
				return false;
			}
		}
		if (!Tiles[newX, newY].Terrain.IsPassable)
		{
			// can't walk through walls
			return false;
		}

		// move the actor
		Tiles[x, y].Actor = null;
		Tiles[newX, newY].Actor = actor;

		// update the map
		Game.Update(Tiles[x, y]);
		Game.Update(Tiles[newX, newY]);

		// collect any treasures
		foreach (var treasure in Tiles[x, y].Treasures.ToArray())
		{
			treasure.OnCollect(this, x, y, actor);
		}

		// schedule next turn
		actor.ScheduleNextTurn();

		// yes, we did take a turn
		return true;
	}

	/// <summary>
	/// Processes the turns of NPCs until it's the player's turn to move again.
	/// </summary>
	public void ProcessNpcTurns()
	{
		while (Game.Player is not null && !Game.Player.Delay.IsEmpty)
		{
			// find all non-player actors whose turn it is
			List<Actor> actors = [];
			foreach (var tile in Tiles)
			{
				if (tile.Actor is not null && !tile.Actor.IsPlayerControlled && tile.Actor.Delay.IsEmpty)
				{
					// don't let creatures move twice by moving to the next tile in the array!
					actors.Add(tile.Actor);
				}
			}

			// let the actors act
			foreach (var actor in actors)
			{
				actor.ActAsEnemy();
			}

			// pass some time
			var allActors = Tiles.Cast<Tile>().Select(q => q.Actor).Where(q => q is not null);
			var ticksToWait = allActors.Min(q => q.TicksToWait);
			foreach (var actor in allActors)
			{
				actor.Wait(ticksToWait);
				if (actor.Delay.IsEmpty)
				{
					// when it's the actor's turn, restore some stamina and mana
					actor.Stamina.Restore(1);
					actor.Mana.Restore(1);
				}
			}
		}
	}
}
