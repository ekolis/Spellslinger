using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// A type of actor.
/// </summary>
public record ActorType(char Character, Color Color)
{
	/// <summary>
	/// Is this actor type controlled by the player?
	/// </summary>
	public bool IsPlayerControlled { get; init; }

	public static ActorType Player { get; } = new ActorType('@', Color.White)
	{
		IsPlayerControlled = true
	};
}
