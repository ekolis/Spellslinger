namespace Spellslinger.Models;

public class GameUpdatedEventArgs
{
	public GameUpdatedEventArgs()
	{

	}
	public GameUpdatedEventArgs(Tile tile)
	{
		Tile = tile;
	}

	/// <summary>
	/// If this update is specific to a tile, this will be it.
	/// </summary>
	public Tile? Tile { get; set; }

	/// <summary>
	/// If a game update is not specific to something, it will be global.
	/// </summary>
	public bool IsGlobal => Tile is null;
}
