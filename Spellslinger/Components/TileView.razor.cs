using Spellslinger.Models;

namespace Spellslinger.Components;

public partial class TileView
{
	protected override void GameUpdated(object sender, GameUpdatedEventArgs e)
	{
		base.GameUpdated(sender, e);
		if (e.Tile == Model)
		{
			StateHasChanged();
		}
	}
}
