namespace Spellslinger.Models;

/// <summary>
/// Measures some attribute of an actor which can fluctuate.
/// </summary>
public class Meter
{
	/// <summary>
	/// Creates a meter and fills it to the maximum.
	/// </summary>
	/// <param name="maximum"></param>
	public Meter(int maximum)
	{
		Maximum = maximum;
		Restore();
	}

	/// <summary>
	/// The current value of the meter.
	/// </summary>
	public int Value { get; set; }

	/// <summary>
	/// The maximum value of the meter.
	/// </summary>
	public int Maximum { get; set; }

	/// <summary>
	/// Depletes this meter.
	/// </summary>
	/// <param name="amount"></param>
	/// <returns>Number of points of deficit.</returns>
	public int Deplete(int amount)
	{
		if (Value < amount)
		{
			var result = amount - Value;
			Value = 0;
			return result;
		}
		else
		{
			Value -= amount;
			return 0;
		}
	}

	/// <summary>
	/// Restores this meter.
	/// </summary>
	/// <param name="amount"></param>
	/// <returns>Number of points of overflow.</returns>
	public int Restore(int amount)
	{
		if (Value + amount > Maximum)
		{
			var result = Value + amount - Maximum;
			Value = Maximum;
			return result;
		}
		else
		{
			Value += amount;
			return 0;
		}
	}

	/// <summary>
	/// Restores this meter completely.
	/// </summary>
	public void Restore()
	{
		Value = Maximum;
	}

	public bool IsEmpty => Value <= 0;

	public bool IsFull => Value >= Maximum;
}
