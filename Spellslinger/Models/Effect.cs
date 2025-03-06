using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// An effect which is displayed temporarily to show the area of effect of a spell or melee attack.
/// </summary>
public class Effect
{
	public char Character { get; set; } = '*';

	public Color Color { get; set; } = Color.White;

	public Color BackgroundColor { get; set; } = Color.Black;
}
