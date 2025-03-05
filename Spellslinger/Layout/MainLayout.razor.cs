namespace Spellslinger.Layout;

public partial class MainLayout
{
	protected override void OnInitialized()
	{
		Game.Music.TrackChanged += TrackChanged;
	}

	private void TrackChanged(object sender, string trackFilename)
	{
		StateHasChanged();
	}
}
