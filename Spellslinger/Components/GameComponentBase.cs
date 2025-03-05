using Microsoft.AspNetCore.Components;
using Spellslinger.Services;

namespace Spellslinger.Components;

/// <summary>
/// Components that has access to game state and receives updates from it.
/// </summary>
public class GameComponentBase
	: ComponentBase
{
	[Inject]
	public IGame Game { get; set; }

	protected override void OnInitialized()
	{
		base.OnInitialized();
		Game.Updated += GameUpdated;
	}

	private void GameUpdated(object sender, EventArgs e)
	{
		StateHasChanged();
	}
}
