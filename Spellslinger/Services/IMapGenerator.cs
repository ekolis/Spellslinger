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
	/// <param name="game"></param>
	/// <param name="depth"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <param name="rooms"></param>
	/// <param name="extraDoors"></param>
	/// <param name="enemies"></param>
	/// <returns></returns>
	public Map Generate(IGame game, int depth, int width, int height, int rooms, int extraDoors, int enemies);
}
