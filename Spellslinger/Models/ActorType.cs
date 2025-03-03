using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// A type of actor.
/// </summary>
public record ActorType(string Name, char Character, Color Color, ActorStats Stats)
{
	/// <summary>
	/// Is this actor type controlled by the player?
	/// </summary>
	public bool IsPlayerControlled { get; init; }

	public static ActorType Player { get; } = new(
		"player",
		'@',
		Color.White,
		new(3, 3, 3, 3, 3))
	{
		IsPlayerControlled = true
	};

	public static ActorType Blob { get; } = new(
		"blob",
		'b',
		Color.Blue,
		new(1, 1, 1, 2, 3));
}
