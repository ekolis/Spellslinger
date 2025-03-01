using Microsoft.AspNetCore.Components.Web;

namespace Spellslinger.Pages;

public partial class Home
{
	protected override void OnInitialized()
	{
		Game.CurrentMap = MapGenerator.Generate(Game, 64, 32, 16, 8);
	}

	protected void KeyPress(KeyboardEventArgs e)
	{
		Game.AcceptKeyboardInput(e);
		StateHasChanged();
	}
}
