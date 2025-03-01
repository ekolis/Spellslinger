namespace Spellslinger.Pages;

public partial class Home
{
	protected override void OnInitialized()
	{
		Game.CurrentMap = MapGenerator.Generate(64, 32, 16, 8);
	}
}
