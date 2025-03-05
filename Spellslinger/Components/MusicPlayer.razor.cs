using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;

namespace Spellslinger.Components;

public partial class MusicPlayer
{
	[Parameter]
	public string? TrackFilename { get; set; }

	public async Task Refresh()
	{
		TrackFilename = Game.Music.CurrentTrack;
		StateHasChanged();
		await js.InvokeVoidAsync("PlayAudioFile", $"Music/{TrackFilename}");
	}
}
