using Microsoft.AspNetCore.Components.Web;
using Spellslinger.Models;

namespace Spellslinger.Services;

public class Game
	: IGame
{
	public required Map CurrentMap { get; set; }

	public Actor? Player => CurrentMap.Player;

	public IList<string> Log { get; } = [];

	public void AcceptKeyboardInput(KeyboardEventArgs e)
	{
		if (Player != null)
		{
			Player.AcceptKeyboardInput(e);
		}
		else
		{
			Log.Add("You don't seem to be alive right now.");
		}
	}
}
