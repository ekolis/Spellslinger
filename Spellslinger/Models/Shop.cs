using Spellslinger.Services;

namespace Spellslinger.Models;

public class Shop
{
	public Shop(IGame game)
	{
		Game = game;
		Restock();
	}

	private IGame Game;

	/// <summary>
	/// The runes that are available for purchase.
	/// </summary>
	public IList<Rune> Runes { get; } = [];

	/// <summary>
	/// The gold cost to search for more items.
	/// </summary>
	public int SearchCost => 20;

	/// <summary>
	/// Searches for more items for purchase.
	/// </summary>
	public void Search()
	{
		if (Game.Player.Gold >= SearchCost)
		{
			Restock();
			Game.Player.Gold -= SearchCost;
		}
	}

	private void Restock()
	{
		for (var i = 0; i < 5; i++)
		{
			var rune = Rune.All.PickWeighted(q => 1, Game.Rng);
			Runes.Add(rune);
		}
		while (Runes.Count > 10)
		{
			Runes.RemoveAt(0);
		}
	}
}
