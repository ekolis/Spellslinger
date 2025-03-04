using Microsoft.AspNetCore.Components;
using Spellslinger.Models;

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
		Game.InputMode = InputMode.Dungeon;
	}
}
