using Spellslinger.Services;

namespace Spellslinger.Models;

public class Shop
{
	public Shop(IGame game)
	{
		Game = game;
		for (var i = 0; i < 3; i++)
		{
			AddRune();
		}
	}

	private IGame Game;

	/// <summary>
	/// The runes that are available for purchase.
	/// </summary>
	public IList<Rune> Runes { get; } = [];

	/// <summary>
	/// The gold cost to search for more items.
	/// </summary>
	public int SearchCost => 10;

	/// <summary>
	/// Searches for more items for purchase.
	/// </summary>
	public void Search()
	{
		if (Game.Player.Gold >= SearchCost)
		{
			AddRune();
			Game.Player.Gold -= SearchCost;
		}
	}

	private void AddRune()
	{
		var rune = Rune.All.PickWeighted(q => 1, Game.Rng);
		Runes.Add(rune);
	}
}
