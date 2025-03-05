namespace Spellslinger.Services;

public class Music
{
	/// <summary>
	/// The filename of the current track. Music is loaded from the music directory.
	/// </summary>
	public string? CurrentTrack { get; private set; }

	/// <summary>
	/// The format of the current track.
	/// </summary>
	public string? CurrentFormat { get; private set; }

	public void Play(string track, string format)
	{
		CurrentTrack = track;
		CurrentFormat = format;
		TrackChanged?.Invoke(this, track);
	}

	public event EventHandler<string>? TrackChanged;
}
