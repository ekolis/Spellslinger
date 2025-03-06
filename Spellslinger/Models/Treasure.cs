using System.Drawing;
using Spellslinger.Services;

namespace Spellslinger.Models;

/// <summary>
/// A treasure that can be collected.
/// </summary>
public abstract class Treasure
{
	protected Treasure(IGame game)
	{
		Game = game;
	}

	protected IGame Game { get; }

	/// <summary>
	/// The name of the treasure.
	/// </summary>
	public abstract string Name { get; }

	/// <summary>
	/// The character used to represent this treasure.
	/// </summary>
	public abstract char Character { get; }

	/// <summary>
	/// The color used to represent this treasure.
	/// </summary>
	public abstract Color Color { get; }

	/// <summary>
	/// Priority for which treasure should be displayed on a tile. Lower numbers are higher priority.
	/// </summary>
	public abstract int Priority { get; }

	/// <summary>
	/// Triggers when the treasure is spawned.
	/// </summary>
	/// <param name="map"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public abstract void OnSpawn(Map map, int x, int y);

	/// <summary>
	/// Triggers when the treasure is stepped on by an actor.
	/// </summary>
	/// <param name="map"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="actor"></param>
	public abstract void OnCollect(Map map, int x, int y, Actor actor);

	public override string ToString()
	{
		return Name;
	}
}
