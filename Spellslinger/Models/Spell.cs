using Spellslinger.Services;

namespace Spellslinger.Models;

/// <summary>
/// A magic spell.
/// </summary>
public abstract class Spell
{
	/// <summary>
	/// Basic stats of the spell. Can be affected by modifiers.
	/// </summary>
	public SpellStats Stats { get; set; }

	/// <summary>
	/// The name of the spell.
	/// </summary>
	public string Name => Stats.Name(Stats);

	/// <summary>
	/// A brief description of the spell's effects.
	/// </summary>
	public string Description => Stats.Description(Stats);

	/// <summary>
	/// Additional details on the spell's effects, such as formulas and stats used.
	/// </summary>
	public string Details => Stats.Details(Stats);

	/// <summary>
	/// The number of MP required to cast the spell.
	/// </summary>
	public int MPCost => Stats.MPCost;

	/// <summary>
	/// Is this spell cast in a particular direction?
	/// </summary>
	public abstract bool IsDirectional { get; }

	/// <summary>
	/// Any modifiers that have been applied to this spell.
	/// </summary>
	public IEnumerable<SpellModifier> Modifiers { get; private set; } = [];

	/// <summary>
	/// Casts the spell.
	/// </summary>
	/// <param name="caster">The actor who is casting the spell.</param>
	/// <param name="dx">The x-component of the direction vector (ignored if non-directional).</param>
	/// <param name="dy">The y-component of the direction vector (ignored if non-directional).</param>
	/// <returns>true if a turn was spent casting the spell, false if the spell couldn't be cast (e.g lack of MP).</returns>
	public bool Cast(IGame game, Actor caster, int dx, int dy)
	{
		if (caster.MP.Value < MPCost)
		{
			game.Log.Add($"The {caster} tries to cast {Name}, but lacks the required MP.");
			return false;
		}
		else
		{
			game.Log.Add($"The {caster} casts {Name}.");
			caster.MP.Deplete(MPCost);
			CastImpl(game, caster, dx, dy);
			caster.ScheduleNextTurn();
			return true;
		}
	}

	protected abstract void CastImpl(IGame game, Actor caster, int dx, int dy);

	public void ApplyModifier(SpellModifier modifier)
	{
		// add the modifier to the list of modifiers on the spell
		Modifiers = [..Modifiers, modifier];

		// apply the modifier to the spell's stats
		Stats = modifier.Apply(Stats);

	}

	public override string ToString()
	{
		return Name;
	}
}
