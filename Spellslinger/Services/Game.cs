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
		Boss = new Actor(ActorType.Bosses.PickWeighted(q => 1, Rng), this);
		Boss.IsHibernating = true;
	}

	public IMapGenerator MapGenerator { get; } = new MapGenerator();
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

	public Music Music { get; } = new Music();

	public bool IsArtifactCollected { get; set; } = false;

	public Actor? Boss { get; set; }

	public void AcceptKeyboardInput(KeyboardEventArgs e)
	{
		if (Player != null)
		{
			Player.AcceptKeyboardInput(e);

			// one last update in case nothing explicitly interesting happened
			Update();
		}
		else
		{
			Log.Add("You don't seem to be alive right now.");
		}
	}

	public void Update()
	{
		Updated?.Invoke(this, new EventArgs());
	}

	public event EventHandler<InputMode> InputModeChanged;

	public event EventHandler Updated;
}
