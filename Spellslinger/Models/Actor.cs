using System.Drawing;
using Microsoft.AspNetCore.Components.Web;
using Spellslinger.Services;

namespace Spellslinger.Models;

/// <summary>
/// An entity in the game which can perform actions.
/// </summary>
public class Actor(ActorType type, IGame game)
{
	/// <summary>
	/// The type of actor that this is.
	/// </summary>
	public ActorType Type { get; set; } = type;

	public char Character => Type.Character;

	public Color Color => Type.Color;

	public bool IsPlayerControlled => Type.IsPlayerControlled;

	/// <summary>
	/// Sends keyboard input to this actor.
	/// </summary>
	/// <param name="e"></param>
	/// <returns>true if the input caused the actor to perform an action that consumed its turn, otherwise false.</returns>
	public bool AcceptKeyboardInput(KeyboardEventArgs e)
	{
		if (IsPlayerControlled)
		{
			switch (e.Code)
			{
				case "ArrowLeft":
					return game.CurrentMap.MoveActor(this, -1, 0);
				case "ArrowRight":
					return game.CurrentMap.MoveActor(this, 1, 0);
				case "ArrowUp":
					return game.CurrentMap.MoveActor(this, 0, -1);
				case "ArrowDown":
					return game.CurrentMap.MoveActor(this, 0, 1);
				default:
					game.Log.Add($"Unknown key pressed: {e.Code}");
					return false;
			}
		}
		else
		{
			return false;
		}
	}
}
