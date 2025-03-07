using Spellslinger.Models;

namespace Spellslinger.Components;

public partial class TitleView
{
	private void BeginGame()
	{
		Game.InputMode = InputMode.CharacterSelection;
	}
}
