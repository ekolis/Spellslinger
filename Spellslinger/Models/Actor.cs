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
		Delay = new Meter(Stats.MaxSpeed);
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
	/// Meter which counts down to the actor's next turn.
	/// </summary>
	public Meter Delay { get; }

	/// <summary>
	/// Sends keyboard input to this actor.
	/// </summary>
	/// <param name="e"></param>
	/// <returns>true if the input caused the actor to perform an action that consumed its turn, otherwise false.</returns>
	public bool AcceptKeyboardInput(KeyboardEventArgs e)
	{
		if (IsPlayerControlled && Delay.IsEmpty)
		{
			var moved = false;
			switch (e.Code)
			{
				case "ArrowLeft":
					moved = Game.CurrentMap.MoveActor(this, -1, 0);
					break;
				case "ArrowRight":
					moved = Game.CurrentMap.MoveActor(this, 1, 0);
					break;
				case "ArrowUp":
					moved = Game.CurrentMap.MoveActor(this, 0, -1);
					break;
				case "ArrowDown":
					moved = Game.CurrentMap.MoveActor(this, 0, 1);
					break;
				default:
					Game.Log.Add($"Unknown key pressed: {e.Code}");
					break;
			}

			if (moved)
			{
				// player moved, allow other actors to move
				Game.CurrentMap.ProcessNpcTurns();
			}

			return moved;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Acts like an enemy for one turn.
	/// </summary>
	public void ActAsEnemy()
	{
		// TODO: move and attack player
		Game.Log.Add($"The {this} says 'boo'!");
		ScheduleNextTurn();
	}

	/// <summary>
	/// Waits one tick for the actor's next turn.
	/// </summary>
	public void Wait()
	{
		Delay.Deplete(Stats.Speed);
	}

	/// <summary>
	/// Schedules the actor's next turn.
	/// </summary>
	public void ScheduleNextTurn()
	{
		Delay.Restore();
	}

	public override string ToString()
	{
		return Type.Name;
	}
}
