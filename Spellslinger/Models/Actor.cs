using System.Drawing;
using Microsoft.AspNetCore.Components.Web;
using Spellslinger.Services;

namespace Spellslinger.Models;

/// <summary>
/// An entity in the game which can perform actions.
/// </summary>
public class Actor
{
	public Actor(ActorType type, IGame game)
	{
		Type = type;
		Game = game;
		HP = new DualMeter(Stats.Toughness * 5, Stats.Toughness + Stats.Willpower);
		MP = new DualMeter(Stats.Memory * 5, Stats.Memory + Stats.Willpower);
	}

	private readonly IGame Game;

	/// <summary>
	/// The type of actor that this is.
	/// </summary>
	public ActorType Type { get; set; } // TODO: when actor type changes, recalculate meters

	public char Character => Type.Character;

	public Color Color => Type.Color;

	public bool IsPlayerControlled => Type.IsPlayerControlled;
	public Stats Stats => Type.Stats; // TODO: modifiable stats for actors

	/// <summary>
	/// The actor's HP meters.
	/// </summary>
	public DualMeter HP { get; }

	/// <summary>
	/// The actor's MP meters.
	/// </summary>
	public DualMeter MP { get; }

	/// <summary>
	/// Regenerating pool of HP.
	/// </summary>
	public Meter Stamina => HP.Outer;

	/// <summary>
	/// Large non-regenerating pool of HP.
	/// </summary>
	public Meter Health => HP.Inner;

	/// <summary>
	/// Regenerating pool of MP.
	/// </summary>
	public Meter Mana => MP.Outer;

	/// <summary>
	/// Large non-regenerating pool of MP.
	/// </summary>
	public Meter Reserves => MP.Inner;

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
					return Game.CurrentMap.MoveActor(this, -1, 0);
				case "ArrowRight":
					return Game.CurrentMap.MoveActor(this, 1, 0);
				case "ArrowUp":
					return Game.CurrentMap.MoveActor(this, 0, -1);
				case "ArrowDown":
					return Game.CurrentMap.MoveActor(this, 0, 1);
				default:
					Game.Log.Add($"Unknown key pressed: {e.Code}");
					return false;
			}
		}
		else
		{
			return false;
		}
	}
}
