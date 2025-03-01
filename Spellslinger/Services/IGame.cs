using Spellslinger.Models;

namespace Spellslinger.Services;

/// <summary>
/// The main game state.
/// </summary>
public interface IGame
{
	/// <summary>
	/// The map on which the player is curerntly located.
	/// </summary>
	public Map CurrentMap { get; set; }
}
