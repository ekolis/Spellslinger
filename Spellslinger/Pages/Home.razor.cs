using Microsoft.AspNetCore.Components.Web;
using Spellslinger.Components;
using Spellslinger.Models;

namespace Spellslinger.Pages;

public partial class Home
{
	private StatsView statsView;

	protected override void OnInitialized()
	{
		Game.InputModeChanged += GameInputModeChanged;
	}

	private void GameInputModeChanged(object? sender, InputMode mode)
	{
		if (mode == InputMode.Spellcrafting)
		{
			// player has left the dungeon, delete it so a new one can be created next time they enter
			Game.CurrentMap = null;
		}
		else if (mode == InputMode.Dungeon && Game.CurrentMap is null)
		{
			// player has entered the dungeon, create a new map
			Game.CurrentMap = MapGenerator.Generate(Game, 1, 64, 32, 16, 8, 16);
			Game.CurrentMap.ProcessNpcTurns();
		}

		StateHasChanged();
		statsView.Refresh();
	}

	protected void KeyDown(KeyboardEventArgs e)
	{
		Game.AcceptKeyboardInput(e);
		StateHasChanged();
		statsView.Refresh();
	}
}
