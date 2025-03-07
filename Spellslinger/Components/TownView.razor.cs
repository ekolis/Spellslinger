using Microsoft.AspNetCore.Components;
using Spellslinger.Models;
using Spellslinger.Services;

namespace Spellslinger.Components;

public partial class TownView
{
	private void Train()
	{
		Game.InputMode = InputMode.Training;
	}

	private void Shop()
	{
		Game.InputMode = InputMode.Shopping;
	}

	private void CraftSpells()
	{
		Game.InputMode = InputMode.Spellcrafting;
	}

	private void EnterDungeon()
	{
		Game.CurrentMap = Game.MapGenerator.Generate(Game, 1, false);
		Game.InputMode = InputMode.Dungeon;
	}

	private int GetRecallCost(int depth)
	{
		return depth * 10;
	}

	private void RecallToDungeon(int depth)
	{
		var cost = GetRecallCost(depth);
		if (Game.Player.Gold >= cost)
		{
			Game.Player.Gold -= cost;
			Game.CurrentMap = Game.MapGenerator.Generate(Game, depth, false);
			Game.InputMode = InputMode.Dungeon;
		}
	}
}
