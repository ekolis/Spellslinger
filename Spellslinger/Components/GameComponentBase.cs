using Microsoft.AspNetCore.Components;
using Spellslinger.Models;
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

	protected virtual void GameUpdated(object sender, GameUpdatedEventArgs e)
	{
		if (e.IsGlobal)
		{
			StateHasChanged();
		}
	}
}
