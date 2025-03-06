using Microsoft.AspNetCore.Components;
using Spellslinger.Models;
using Spellslinger.Services;

namespace Spellslinger.Components;

public partial class ShopView
{
	private void BuyRune(Rune rune)
	{
		if (Game.Player.Gold >= rune.Cost * Game.RuneCostFactor)
		{
			Game.Player.Knowledge.Runes.Add(rune);
			Game.Shop.Runes.Remove(rune);
			Game.Player.Gold -= rune.Cost * Game.RuneCostFactor;
		}
	}

	private void ReturnToTown()
	{
		Game.InputMode = InputMode.Town;
	}
}
