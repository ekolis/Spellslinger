using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Spellslinger.Components;

public partial class MusicPlayer
{
	[Parameter]
	public string? TrackFilename { get; set; }
}
