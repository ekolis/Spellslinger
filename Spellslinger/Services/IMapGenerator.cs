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
	/// <param name="rooms"></param>
	/// <param name="extraDoors"></param>
	/// <returns></returns>
	public Map Generate(IGame game, int width, int height, int rooms, int extraDoors);
}
