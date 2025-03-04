using Spellslinger.Models;
using Spellslinger.Services;

namespace Spellslinger.Components;

public partial class CharacterSelectionView
{
	private void StartGame(ActorType actorType)
	{
		Game.Player = new Actor(actorType, Game);
		Game.Player.IsPlayerControlled = true;
		Game.InputMode = InputMode.Town;
	}
}
