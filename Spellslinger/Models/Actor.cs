using System.Drawing;
using Microsoft.AspNetCore.Components.Web;
using Spellslinger.Models.Spells;
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
		Stats = Type.Stats;
		Game = game;
		HP = new DualMeter(Stats.Toughness * 5, Stats.Toughness + Stats.Willpower);
		MP = new DualMeter(Stats.Memory * 5, Stats.Memory + Stats.Willpower);
		Delay = new Meter(ActorStats.MaxSpeed);
		foreach (var rune in type.Runes)
		{
			Knowledge.Runes.Add(rune);
		}
		if (Knowledge.Runes.Contains(Rune.Force))
		{
			// slot the force spell as a default
			var spell = Rune.Force.Spell;
			Knowledge.Spells.Add(spell);
			Knowledge.MeleeSpells.Add(spell);
		}
		var remainingRunes = Knowledge.Runes.Except(new[] { Rune.Force }).ToList();
		for (var i = 0; i < MaxGeneralSpells; i++)
		{
			// slot a general spell
			if (remainingRunes.Any(q => q.Spell != null))
			{
				var rune = remainingRunes.First(q => q.Spell != null);
				var spell = rune.Spell;
				Knowledge.Spells.Add(spell);
				Knowledge.GeneralSpells.Add(spell);
				remainingRunes.Remove(rune);
			}
		}
		Experience = type.Experience;
		Gold = type.Gold;
	}

	private readonly IGame Game;

	/// <summary>
	/// The type of actor that this is.
	/// </summary>
	public ActorType Type { get; set; } // TODO: when actor type changes, recalculate meters

	public char Character => Type.Character;

	public Color Color => Type.Color;

	public bool IsPlayerControlled { get; set; }

	public ActorStats Stats { get; set; }

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
	/// What this actor knows.
	/// </summary>
	public Knowledge Knowledge { get; } = new Knowledge();

	/// <summary>
	/// Unspent experience points.
	/// </summary>
	public int Experience { get; set; }

	/// <summary>
	/// Unspent gold pieces.
	/// </summary>
	public int Gold { get; set; }

	[Obsolete("Use Knowledge.MeleeSpells.")]
	private IList<Spell> MeleeSpells => Knowledge.MeleeSpells;

	[Obsolete("Use Knowledge.GeneralSpells.")]
	private IList<Spell> GeneralSpells => Knowledge.GeneralSpells;

	/// <summary>
	/// The maximum number of melee spells that this actor can have prepared at a time.
	/// </summary>
	public int MaxMeleeSpells => 1 + Stats.Strength / 5;

	/// <summary>
	/// The maximum number of general spells that this actor can have prepared at a time.
	/// </summary>
	public int MaxGeneralSpells => 1 + Stats.Memory / 5;

	/// <summary>
	/// The maximum number of runes that this actor can use in a single spell.
	/// </summary>
	public int MaxRunesPerSpell => 1 + Stats.Willpower / 3;

	public IEnumerable<Element> Resistances => Type.Resistances; // TODO: temporary resistances

	public IEnumerable<Element> Vulnerabilities => Type.Vulnerabilities; // TODO: temporary resistances

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
					moved = HandleDirectionalInput(-1, 0);
					break;
				case "ArrowRight":
					moved = HandleDirectionalInput(1, 0);
					break;
				case "ArrowUp":
					moved = HandleDirectionalInput(0, -1);
					break;
				case "ArrowDown":
					moved = HandleDirectionalInput(0, 1);
					break;
				case "KeyZ":
					moved = PrepareCastPlayerSpell(0);
					break;
				case "KeyX":
					moved = PrepareCastPlayerSpell(1);
					break;
				case "KeyC":
					moved = PrepareCastPlayerSpell(2);
					break;
				case "KeyV":
					moved = PrepareCastPlayerSpell(3);
					break;
				case "KeyB":
					moved = PrepareCastPlayerSpell(4);
					break;
				case "KeyN":
					moved = PrepareCastPlayerSpell(5);
					break;
				case "KeyM":
					moved = PrepareCastPlayerSpell(6);
					break;
				case "Space":
					moved = InteractWithTerrain();
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
		// can't act while hibernating
		if (IsHibernating)
		{
			ScheduleNextTurn();
			return;
		}

		var myLocation = Game.CurrentMap.LocateActor(this);
		var playerLocation = Game.CurrentMap.LocateActor(Game.Player);
		var dx = playerLocation.x - myLocation.x;
		var dy = playerLocation.y - myLocation.y;

		// randomly cast spells when player is nearby with a chance depending on Willpower and Memory
		// TODO: check range of particular spell
		// TODO: fog of war
		var spells = Knowledge.GeneralSpells.Where(q => q.MPCost <= MP.Value);
		if ((dx == 0 || dy == 0) && dx <= 3 && dy <= 3 && spells.Any() && Game.Rng.Next(0, 100) < Stats.Willpower + Stats.Memory)
		{
			// try to cast a random available spell
			var spell = spells.PickWeighted(q => 1, Game.Rng);
			spell.Cast(Game, this, Math.Sign(dx), Math.Sign(dy));
		}
		else if (dx == 1 && dy == 0
			|| dx == -1 && dy == 0
			|| dx == 0 && dy == 1
			|| dx == 0 && dy == -1)
		{
			// player is directly adjacent, attack!
			Attack(Game.Player);
		}
		else
		{
			// move toward player
			// TODO: fog of war
			var verticalChance = (double)Math.Abs(dy) / Math.Abs(dx + dy);
			var goVertical = Game.Rng.NextDouble() < verticalChance;
			if (goVertical)
			{
				Game.CurrentMap.MoveActor(this, 0, Math.Sign(dy));
			}
			else
			{
				Game.CurrentMap.MoveActor(this, Math.Sign(dx), 0);
			}
		}

		ScheduleNextTurn();
	}

	/// <summary>
	/// Waits for the actor's next turn.
	/// </summary>
	public void Wait(int ticks)
	{
		Delay.Deplete(Stats.Speed * ticks);
	}

	/// <summary>
	/// Schedules the actor's next turn.
	/// </summary>
	public void ScheduleNextTurn()
	{
		Delay.Restore();
	}

	/// <summary>
	/// The number of ticks the actor must wait before its next turn.
	/// </summary>
	public int TicksToWait => (int)Math.Ceiling((double)Delay.Value / Stats.Speed);

	/// <summary>
	/// Performs a melee attack on a target, then casts any melee spells that are slotted.
	/// </summary>
	/// <remarks>
	/// This function doesn't check that the target is in melee attack range. You should do that before calling it.
	/// </remarks>
	/// <param name="target"></param>
	public void Attack(Actor target)
	{
		// melee attack
		var damage = Stats.Strength;
		var critChance = Stats.Willpower;
		var isCrit = Game.Rng.Next(0, 100) < critChance;
		var verb = "hits";
		if (isCrit)
		{
			damage *= 2;
			verb = "critcally hits";
		}
		Game.Log.Add($"The {this} {verb} the {target} ({damage} damage).");
		target.TakeDamage(damage, this, SpellTags.None, null); // TODO: spell tags for melee attacks?

		// cast spells if the target isn't dead
		if (target.IsAlive)
		{
			var myPos = Game.CurrentMap.LocateActor(this);
			var targetPos = Game.CurrentMap.LocateActor(target);
			foreach (var spell in MeleeSpells)
			{
				spell.Cast(Game, this, targetPos.x - myPos.x, targetPos.y - myPos.y);
			}
		}
	}

	/// <summary>
	/// Takes some damage, potentially killing the actor.
	/// </summary>
	/// <param name="damage">The amount of damage.</param>
	/// <param name="attacker">Who's inflicting the damage?</param>
	/// <param name="tags">Any spell tags to apply to the damage.</param>
	/// <param name="element">The element of the attack.</param>
	public void TakeDamage(int damage, Actor attacker, SpellTags tags, Element? element)
	{
		if (element is not null)
		{
			if (Resistances.Contains(element) && !Vulnerabilities.Contains(element))
			{
				Game.Log.Add($"The {this} resists the {element.Description}.");
				damage /= 2;
			}
			if (Vulnerabilities.Contains(element) && !Resistances.Contains(element))
			{
				Game.Log.Add($"The {this} is vulnerable to the {element.Description}!");
				damage *= 3;
				damage /= 2;
			}
		}

		int leftoverDamage;
		if (tags.HasFlag(SpellTags.StaminaOnly))
		{
			leftoverDamage = Stamina.Deplete(damage);
		}
		else
		{
			leftoverDamage = HP.Deplete(damage);
		}

		Awaken();

		if (Stamina.Value <= 0)
		{
			Game.Log.Add($"The {this} is exhausted.");
			if (tags.HasFlag(SpellTags.StaminaOnly))
			{
				Game.Log.Add($"{leftoverDamage} damage was not applied to the {this}'s health because this spell affects only stamina.");
			}
		}

		if (HP.Value <= 0)
		{
			Game.Log.Add($"The {this} is killed!");
			// TODO: maybe leave gold on the floor for other actors to steal?
			Game.Log.Add($"The {attacker} gains the {this}'s {Experience} experience and {Gold} gold.");
			attacker.Gold += Gold;
			attacker.Experience += Experience;
			foreach (var rune in Knowledge.Runes)
			{
				// percentage chance based on memory stat to learn a rune from a defeated foe
				if (Game.Rng.Next(0, 100) < attacker.Stats.Memory)
				{
					Game.Log.Add($"The {attacker} learns the {rune} rune!");
					attacker.Knowledge.Runes.Add(rune);
				}
			}
			var tile = Game.CurrentMap.LocateActor(this);
			Game.CurrentMap.Tiles[tile.x, tile.y].Actor = null;
			if (IsPlayerControlled)
			{
				Game.Log.Add("Game over...");
				Game.Player = null;
			}
		}
	}

	private bool PrepareCastPlayerSpell(int index)
	{
		if (GeneralSpells.Count > index)
		{
			if (GeneralSpells[index].IsDirectional)
			{
				Game.InputMode = InputMode.SpellDirection;
				Game.InputSpell = GeneralSpells[index];
				Game.Log.Add($"Select a direction to cast {GeneralSpells[index].Name}.");
				return false;
			}
			else
			{
				return GeneralSpells[0].Cast(Game, this, 0, 0);
			}
		}
		else
		{
			Game.Log.Add("You haven't prepared a spell in that slot.");
			return false;
		}
	}

	private bool HandleDirectionalInput(int dx, int dy)
	{
		if (Game.InputMode == InputMode.SpellDirection)
		{
			// cast the input spell in the desired direction
			var result = Game.InputSpell.Cast(Game, this, dx, dy);
			Game.InputMode = InputMode.Dungeon;
			Game.InputSpell = null;
			return result;
		}
		else if (Game.InputMode == InputMode.Dungeon)
		{
			// move the actor
			var result = Game.CurrentMap.MoveActor(this, dx, dy);

			if (result
				&& Game.IsArtifactCollected
				&& (Game.Boss?.IsAlive ?? false)
				&& Game.CurrentMap.FindActor(q => q == Game.Boss) is null) // don't spawn infinite bosses!
			{
				// if there is any rubble and the boss is alive but not on this level,
				// there's a chance to respawn the boss there
				foreach (var tile in Game.CurrentMap.Tiles)
				{
					var chance = (double)Game.Boss.Stats.Speed / ((Game.Player?.Stats.Speed ?? 1) + Game.Boss.Stats.Speed) / 10;
					if (tile.Terrain == Terrain.Rubble && tile.Actor is null && Game.Rng.NextDouble() < chance)
					{
						Game.Log.Add($"The {Game.Boss} emerges from the rubble!");
						tile.Actor = Game.Boss;
					}
				}
			}

			return result;
		}
		else
		{
			return false;
		}
	}

	public bool InteractWithTerrain()
	{
		if (Game.InputMode == InputMode.Dungeon)
		{
			// find the terrain
			var (x, y) = Game.CurrentMap.LocateActor(this);
			var terrain = Game.CurrentMap.Tiles[x, y].Terrain;
			if (terrain == Terrain.StairsUp)
			{
				if (IsPlayerControlled)
				{
					// go to the previous dungeon level (or town if on level 1)
					if (Game.CurrentMap.Depth == 1)
					{
						Game.Log.Add("You return to the safety of the town.");
						Game.CurrentMap = null;
						Game.InputMode = InputMode.Town;
						HP.Restore();
						MP.Restore();
						if (Game.IsArtifactCollected)
						{
							// you win by leaving the dungeon with the artifact!
							Game.Log.Add("You have successfully retrieved the Orb of MacGuffin from the dungeon!");
							Game.Log.Add("The orb's power seals away the evil within the dungeon.");
							Game.Log.Add("CONGRATULATIONS! You win!");
							Game.InputMode = InputMode.Victory;
						}
					}
					else
					{
						Game.Log.Add("You return to the previous level. It seems different somehow...");
						Game.CurrentMap = Game.MapGenerator.Generate(Game, Game.CurrentMap.Depth - 1, true);

						if (Game.IsArtifactCollected)
						{
							// destroy the stairs
							Game.Log.Add("The stairs collapse behind you!");
							foreach (var tile in Game.CurrentMap.Tiles)
							{
								if (tile.Terrain == Terrain.StairsDown)
								{
									tile.Terrain = Terrain.Rubble;
								}
							}
						}
					}
					return true;
				}
				else
				{
					if (Game.CurrentMap.Depth > 1)
					{
						// enemy runs away
						Game.CurrentMap.Tiles[x, y].Actor = null;
						Game.Log.Add($"The {this} flees up the stairs.");
						return true;
					}
					else
					{
						Game.Log.Add($"The {this} tries to flee up the stairs, but is blocked by a protective ward guarding the town.");
						return false;
					}
				}
			}
			else if (terrain == Terrain.StairsDown)
			{
				if (IsPlayerControlled)
				{
					// go to the next dungeon level
					Game.Log.Add("You proceed to the next level.");
					Game.CurrentMap = Game.MapGenerator.Generate(Game, Game.CurrentMap.Depth + 1, false);
					return true;
				}
				else
				{
					// enemy runs away
					Game.CurrentMap.Tiles[x, y].Actor = null;
					Game.Log.Add($"The {this} flees down the stairs.");
					return true;
				}
			}
			else
			{
				if (IsPlayerControlled)
				{
					Game.Log.Add("There's nothing here to interact with.");
				}
				return false;
			}
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Is this actor still alive?
	/// </summary>
	public bool IsAlive => HP.Value > 0;

	/// <summary>
	/// Is the actor hibernating? If so, they will not get any turns until awakened.
	/// </summary>
	public bool IsHibernating { get; set; }

	/// <summary>
	/// Awakens the actor.
	/// </summary>
	public void Awaken()
	{
		if (IsHibernating)
		{
			Game.Log.Add($"The {this} awakens from hibernation!");
			IsHibernating = false;
		}
	}

	public override string ToString()
	{
		return Type.Name;
	}
}
