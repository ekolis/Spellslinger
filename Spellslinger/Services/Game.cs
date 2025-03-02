using Microsoft.AspNetCore.Components.Web;
using Spellslinger.Models;

namespace Spellslinger.Services;

public class Game
	: IGame
{
	public required Map CurrentMap { get; set; }

	public Actor? Player
	{
		get => CurrentMap.Player;
		set => CurrentMap.Player = value;
	}

	public IList<string> Log { get; } = [];

	public Random Rng { get; } = new Random();

	public string SpellKeys => "ZXCVBNM";

	public InputMode InputMode { get; set; } = InputMode.Default;

	public Spell InputSpell { get; set; }

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
