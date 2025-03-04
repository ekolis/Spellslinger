using Microsoft.AspNetCore.Components.Web;
using Spellslinger.Models;
using Spellslinger.Models.Spells;

namespace Spellslinger.Services;

public class Game
	: IGame
{
	public Game()
	{
		Shop = new Shop(this);
	}

	public required Map CurrentMap { get; set; }

	public Actor? Player { get; set; }

	public IList<string> Log { get; } = [];

	public Random Rng { get; } = new Random();

	public string SpellKeys => "ZXCVBNM";

	private InputMode inputMode = InputMode.CharacterSelection;

	public InputMode InputMode
	{
		get => inputMode;
		set
		{
			inputMode = value;
			InputModeChanged?.Invoke(this, inputMode);
		}
	}

	public Spell? InputSpell { get; set; }

	public Shop Shop { get; }

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

	public event EventHandler<InputMode> InputModeChanged;
}
