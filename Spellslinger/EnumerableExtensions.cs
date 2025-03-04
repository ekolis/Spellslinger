namespace Spellslinger;

public static class EnumerableExtensions
{
	/// <summary>
	/// Picks a random item from a list, weighting items according to a function.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list">The items to pick from.</param>
	/// <param name="weight">The weighting function.</param>
	/// <param name="rng">A random number generator to use.</param>
	/// <returns>The selected item.</returns>
	/// <exception cref="InvalidOperationException">if an item couldn't be picked.</exception>
	public static T PickWeighted<T>(this IEnumerable<T> list, Func<T, double> weight, Random rng)
	{
		var weights = list.Select(q => new { Item = q, Weight = weight(q) });
		var maxWeight = weights.Sum(q => q.Weight);
		var num = rng.NextDouble() * maxWeight;
		double threshold = 0;
		foreach (var item in weights)
		{
			threshold += item.Weight;
			if (num < threshold)
			{
				return item.Item;
			}
		}
		throw new InvalidOperationException($"Coudldn't pick an item from a weighted list. Number of items: {list.Count()}. Total weight: {threshold}.");
	}
}
