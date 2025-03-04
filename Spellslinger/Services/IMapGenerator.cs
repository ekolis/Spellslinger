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
	/// <returns></returns>
	public Map Generate(IGame game, int depth);
}
