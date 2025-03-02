using Spellslinger.Services;

namespace Spellslinger.Models;

/// <summary>
/// A magic spell.
/// </summary>
public abstract class Spell
{
	public Spell(IGame game)
	{
		Game = game;
	}

	/// <summary>
	/// The current game.
	/// </summary>
	public IGame Game { get; }

	/// <summary>
	/// The name of the spell.
	/// </summary>
	public abstract string Name { get; }

	/// <summary>
	/// A brief description of the spell's effects.
	/// </summary>
	public abstract string Description { get; }

	/// <summary>
	/// Additional details on the spells effects, such as formulas and stats used.
	/// </summary>
	public abstract string Details { get; }

	/// <summary>
	/// Is this spell cast in a particular direction?
	/// </summary>
	public abstract bool IsDirectional { get; }

	/// <summary>
	/// The number of MP required to cast the spell.
	/// </summary>
	public abstract int MPCost { get; }

	/// <summary>
	/// Casts the spell.
	/// </summary>
	/// <param name="caster">The actor who is casting the spell.</param>
	/// <param name="dx">The x-component of the direction vector (ignored if non-directional).</param>
	/// <param name="dy">The y-component of the direction vector (ignored if non-directional).</param>
	/// <returns>true if a turn was spent casting the spell, false if the spell couldn't be cast (e.g lack of MP).</returns>
	public bool Cast(Actor caster, int dx, int dy)
	{
		if (caster.MP.Value < MPCost)
		{
			Game.Log.Add($"The {caster} tries to cast {Name}, but lacks the required MP.");
			return false;
		}
		else
		{
			Game.Log.Add($"The {caster} casts {Name}.");
			caster.MP.Deplete(MPCost);
			CastImpl(caster, dx, dy);
			caster.ScheduleNextTurn();
			return true;
		}
	}

	protected abstract void CastImpl(Actor caster, int dx, int dy);

	public override string ToString()
	{
		return Name;
	}
}
