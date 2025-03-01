using Spellslinger.Models;

namespace Spellslinger.Services;

public class MapGenerator
	: IMapGenerator
{
	public Map Generate(int width, int height)
	{
		var map = new Map(width, height);
		// TODO: generate the map
		return map;
	}
}
