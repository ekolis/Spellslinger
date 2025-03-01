using Spellslinger.Models;

namespace Spellslinger.Services;

/// <summary>
/// Generates a <see cref="Map"/>.
/// </summary>
public interface IMapGenerator
{
	/// <summary>
	/// Genrerates a <see cref="Map"/>.
	/// </summary>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <returns></returns>
	public Map Generate(int width, int height);
}
