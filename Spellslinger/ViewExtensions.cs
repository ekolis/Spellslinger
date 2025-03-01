using System.Drawing;

namespace Spellslinger;

public static class ViewExtensions
{
	public static string ToRgbaString(this Color color)
	{
		return $"rgba({color.R}, {color.G}, {color.B}, {color.A / 255f})";
	}
}
