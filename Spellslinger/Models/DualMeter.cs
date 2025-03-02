namespace Spellslinger.Models;

/// <summary>
/// Two meters stacked on top of each other.
/// </summary>
public class DualMeter
{
	public DualMeter(int innerMaximum, int outerMaximum)
	{
		Inner = new Meter(innerMaximum);
		Outer = new Meter(outerMaximum);
	}

	/// <summary>
	/// The inner meter. It will be depleted first and restored last. This should be larger and non-regenerating.
	/// </summary>
	public Meter Inner { get; }

	/// <summary>
	/// The outer meter. It will be depleted first and restored last. This should be smaller and regenerating.
	/// </summary>
	public Meter Outer { get; }

	/// <summary>
	/// The total value of the two meters.
	/// </summary>
	public int Value => Inner.Value + Outer.Value;

	/// <summary>
	/// The total maximum of the two meters.
	/// </summary>
	public int Maximum => Inner.Maximum + Outer.Maximum;

	/// <summary>
	/// Depletes this meter. The outer meter will be depleted first.
	/// </summary>
	/// <param name="amount"></param>
	/// <returns>Number of points of deficit.</returns>
	public int Deplete(int amount)
	{
		var leftover = Outer.Deplete(amount);
		return Inner.Deplete(leftover);
	}

	/// <summary>
	/// Restores this meter. The inner meter will be restored first.
	/// </summary>
	/// <param name="amount"></param>
	/// <returns>Number of points of overflow.</returns>
	public int Restore(int amount)
	{
		var leftover = Inner.Restore(amount);
		return Outer.Restore(leftover);
	}

	/// <summary>
	/// Restores this meter completely.
	/// </summary>
	public void Restore()
	{
		Inner.Restore();
		Outer.Restore();
	}
}
