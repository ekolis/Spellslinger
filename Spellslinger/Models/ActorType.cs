using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// A type of actor.
/// </summary>
public record ActorType(string Name, char Character, Color Color, ActorStats Stats, IEnumerable<Rune> Runes)
{
	public static ActorType Pyro { get; } = new(
		"pyro mage",
		'@',
		Element.Fire.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Fire]);

	public static ActorType Cryo { get; } = new(
		"cryo mage",
		'@',
		Element.Ice.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Ice]);

	public static ActorType Aero { get; } = new(
		"aero mage",
		'@',
		Element.Air.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Air]);

	public static ActorType Geo { get; } = new(
		"geo mage",
		'@',
		Element.Earth.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Earth]);

	public static ActorType Vis { get; } = new(
		"vis mage",
		'@',
		Element.Force.Color,
		new(5, 3, 3, 3, 3),
		[Rune.Force]);

	public static ActorType Magus { get; } = new(
		"magus",
		'@',
		Color.Gray,
		new(1, 3, 3, 3, 3),
		[Rune.Force, Rune.Fire, Rune.Ice, Rune.Air, Rune.Earth]);

	public static ActorType Blob { get; } = new(
		"blob",
		'b',
		Color.Blue,
		new(2, 1, 1, 2, 3),
		[]);
}
