using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// A type of actor.
/// </summary>
public record ActorType(string Name, char Character, Color Color, ActorStats Stats, IEnumerable<Rune> Runes, int Depth, int Experience, int Gold)
{
	public static ActorType Pyro { get; } = new(
		"pyro mage",
		'@',
		Element.Fire.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Fire, Rune.Fire],
		3,
		10,
		20);

	public static ActorType Cryo { get; } = new(
		"cryo mage",
		'@',
		Element.Ice.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Ice, Rune.Ice],
		3,
		10,
		20);

	public static ActorType Aero { get; } = new(
		"aero mage",
		'@',
		Element.Air.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Air, Rune.Air],
		3,
		10,
		20);

	public static ActorType Geo { get; } = new(
		"geo mage",
		'@',
		Element.Earth.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Earth, Rune.Earth],
		3,
		10,
		20);

	public static ActorType Vis { get; } = new(
		"vis mage",
		'@',
		Element.Force.Color,
		new(5, 3, 3, 3, 3),
		[Rune.Force],
		3,
		10,
		20);

	public static ActorType Magus { get; } = new(
		"magus",
		'@',
		Color.Gray,
		new(1, 3, 3, 3, 3),
		[Rune.Force, Rune.Fire, Rune.Ice, Rune.Air, Rune.Earth],
		3,
		30,
		20);

	public static IEnumerable<ActorType> PlayerCharacters { get; } =
		[Pyro, Cryo, Aero, Vis, Magus];

	public static ActorType Blob { get; } = new(
		"blob",
		'b',
		Color.Blue,
		new(2, 1, 1, 2, 3),
		[],
		1,
		1,
		1);

	public static ActorType Hedgehog { get; } = new(
		"hedgehog",
		'h',
		Color.Orange,
		new(2, 1, 1, 1, 5),
		[],
		1,
		1,
		1);

	public static ActorType Imp { get; } = new(
		"imp",
		'i',
		Color.Red,
		new(3, 2, 2, 2, 4),
		[Rune.Fire],
		2,
		4,
		5);

	public static ActorType Slug { get; } = new(
		"slug",
		's',
		Color.Green,
		new(2, 2, 2, 5, 2),
		[],
		2,
		4,
		3);

	public static IEnumerable<ActorType> Enemies { get; } =
		[Blob, Hedgehog, Imp, Slug];
}
