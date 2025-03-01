using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// An entity in the game which can perform actions.
/// </summary>
public class Actor(ActorType type)
{
	/// <summary>
	/// The type of actor that this is.
	/// </summary>
	public ActorType Type { get; set; } = type;

	public char Character => Type.Character;

	public Color Color => Type.Color;
}
