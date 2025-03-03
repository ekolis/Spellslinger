using Microsoft.AspNetCore.Components.Web;
using Spellslinger.Models;
using Spellslinger.Models.Spells;

namespace Spellslinger.Services;

public class Game
	: IGame
{
	public Game()
	{
		// create a player
		Player = new Actor(ActorType.Player, this);

		// set up some default runes for the player
		// TODO: random or player selected runes?
		Player.Knowledge.Runes.Add(Rune.Force);
		Player.Knowledge.Runes.Add(Rune.Fire);
		Player.Knowledge.Runes.Add(Rune.Ice);
		Player.Knowledge.Runes.Add(Rune.Air);
		Player.Knowledge.Runes.Add(Rune.Earth);

		// set up some default spells for the player
		// TODO: base the spells on the selected runes
		Spell forceFist = new ForceFist();
		forceFist = forceFist.ApplyModifier(SpellModifier.Fire);
		Player.Knowledge.Spells.Add(forceFist);
		Player.Knowledge.MeleeSpells.Add(forceFist);
		Spell fireWave = new FireWave();
		fireWave.ApplyModifier(SpellModifier.Force);
		Player.Knowledge.Spells.Add(fireWave);
		Player.Knowledge.GeneralSpells.Add(fireWave);

		// start in spellcrafting mode
		InputMode = InputMode.Spellcrafting;
	}

	public required Map CurrentMap { get; set; }

	public Actor? Player { get; set; }

	public IList<string> Log { get; } = [];

	public Random Rng { get; } = new Random();

	public string SpellKeys => "ZXCVBNM";

	private InputMode inputMode = InputMode.Spellcrafting;

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
