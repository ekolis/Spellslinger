using Spellslinger.Models;

namespace Spellslinger.Services;

/// <summary>
/// Generates a <see cref="Map"/>.
/// </summary>
public interface IMapGenerator
{
	/// <summary>
	/// Generates a <see cref="Map"/>.
	/// </summary>
	/// <param name="game"></param>
	/// <param name="depth"></param>
	/// <param name="ascending">Did the player ascend or descend to enter this map?</param>
	/// <returns></returns>
	public Map Generate(IGame game, int depth, bool ascending);
}
