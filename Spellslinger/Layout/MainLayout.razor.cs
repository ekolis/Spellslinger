using Spellslinger.Components;

namespace Spellslinger.Layout;

public partial class MainLayout
{
	private MusicPlayer musicPlayer;

	protected override void OnInitialized()
	{
		Game.Music.TrackChanged += TrackChanged;
	}

	private async void TrackChanged(object sender, string trackFilename)
	{
		await musicPlayer.Refresh();
	}
}
