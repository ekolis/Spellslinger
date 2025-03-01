using Spellslinger.Models;

namespace Spellslinger.Services;

public class Game
	: IGame
{
	public required Map CurrentMap { get; set; }
}
