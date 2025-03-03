namespace Spellslinger.Models;

public record ActorStats(int Strength, int Willpower, int Memory, int Toughness, int Speed)
{
	/// <summary>
	/// The maximum speed that any actor can be in the game.
	/// </summary>
	public const int MaxSpeed = 1000;
}