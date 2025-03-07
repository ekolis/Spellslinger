using System.Drawing;

namespace Spellslinger.Models;

/// <summary>
/// A type of actor.
/// </summary>
public record ActorType(string Name, char Character, Color Color, ActorStats Stats, IEnumerable<Rune> Runes, int Depth, int Experience, int Gold, IEnumerable<Element> Resistances, IEnumerable<Element> Vulnerabilities)
{
	public static ActorType Pyro { get; } = new(
		"pyro mage",
		'@',
		Element.Fire.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Fire, Rune.Fire],
		3,
		0,
		0,
		[Element.Fire],
		[Element.Ice]);

	public static ActorType Cryo { get; } = new(
		"cryo mage",
		'@',
		Element.Ice.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Ice, Rune.Ice],
		3,
		0,
		0,
		[Element.Ice],
		[Element.Fire]);

	public static ActorType Aero { get; } = new(
		"aero mage",
		'@',
		Element.Air.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Air, Rune.Air],
		3,
		0,
		0,
		[Element.Air],
		[Element.Earth]);

	public static ActorType Geo { get; } = new(
		"geo mage",
		'@',
		Element.Earth.Color,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Earth, Rune.Earth],
		3,
		0,
		0,
		[Element.Earth],
		[Element.Air]);

	public static ActorType Vis { get; } = new(
		"vis mage",
		'@',
		Element.Force.Color,
		new(5, 3, 3, 3, 3),
		[Rune.Force],
		3,
		0,
		0,
		[],
		[]);

	public static ActorType Magus { get; } = new(
		"magus",
		'@',
		Color.Gray,
		new(2, 3, 5, 2, 3),
		[Rune.Force, Rune.Fire, Rune.Ice, Rune.Air, Rune.Earth],
		3,
		0,
		0,
		[],
		[]);

	public static ActorType Vegan { get; } = new(
		"vegan mage",
		'@',
		Color.Green,
		new(3, 4, 3, 2, 3),
		[Rune.Vegan, Rune.Earth],
		3,
		0,
		0,
		[],
		[]);

	public static ActorType Warp { get; } = new(
		"warp mage",
		'@',
		Color.Purple,
		new(3, 3, 3, 3, 3),
		[Rune.Force, Rune.Warp, Rune.Ascend, Rune.Descend],
		3,
		0,
		0,
		[],
		[]);

	public static IEnumerable<ActorType> PlayerCharacters { get; } =
		[Pyro, Cryo, Aero, Vis, Magus, Vegan, Warp];

	public static ActorType Blob { get; } = new(
		"blob",
		'b',
		Color.Blue,
		new(2, 1, 1, 2, 3),
		[],
		1,
		1,
		1,
		[],
		[]);

	public static ActorType Hedgehog { get; } = new(
		"hedgehog",
		'h',
		Color.Orange,
		new(2, 1, 1, 1, 5),
		[],
		1,
		1,
		1,
		[],
		[]);

	public static ActorType Imp { get; } = new(
		"imp",
		'i',
		Element.Fire.Color,
		new(3, 2, 2, 2, 4),
		[Rune.Fire],
		2,
		4,
		5,
		[Element.Fire],
		[]);

	public static ActorType Slug { get; } = new(
		"slug",
		's',
		Color.Green,
		new(2, 2, 2, 5, 2),
		[],
		2,
		4,
		3,
		[Element.Earth],
		[Element.Ice]);

	public static ActorType IceGolem { get; } = new(
		"ice golem",
		'G',
		Element.Ice.Color,
		new(3, 3, 1, 6, 2),
		[Rune.Ice],
		3,
		10,
		10,
		[Element.Ice],
		[Element.Fire]);

	public static ActorType EarthGolem { get; } = new(
		"earth golem",
		'G',
		Element.Earth.Color,
		new(4, 3, 1, 10, 1),
		[Rune.Earth],
		3,
		8,
		12,
		[Element.Earth],
		[Element.Ice]);

	public static ActorType AeolianVortex { get; } = new(
		"Aeolian vortex",
		'v',
		Element.Air.Color,
		new(4, 6, 6, 2, 5),
		[Rune.Ice],
		4,
		15,
		12,
		[Element.Air],
		[]);

	public static ActorType Ninja { get; } = new(
		"ninja",
		'N',
		Color.Gray,
		new(4, 4, 2, 3, 4),
		[Rune.Force],
		4,
		12,
		20,
		[],
		[]);

	public static ActorType DoomFungus { get; } = new(
		"doom fungus",
		'f',
		Color.Purple,
		new(2, 10, 5, 8, 1),
		[Rune.Force, Rune.Earth, Rune.Ice],
		5,
		20,
		15,
		[Element.Fire],
		[Element.Ice]);

	public static ActorType Xenosphere { get; } = new(
		"xenosphere",
		'x',
		Color.Purple,
		new(3, 8, 10, 8, 2),
		[Rune.Force, Rune.Fire, Rune.Air],
		5,
		25,
		10,
		[Element.Ice],
		[Element.Earth]);

	public static ActorType MadSorcerer { get; } = new(
		"mad sorcerer",
		'S',
		Color.Pink,
		new(3, 10, 10, 5, 3),
		[Rune.Force, Rune.Fire, Rune.Ice, Rune.Air, Rune.Earth],
		6,
		50,
		5,
		[],
		[]);

	public static ActorType Butcher { get; } = new(
		"butcher",
		'B',
		Color.Red,
		new(12, 6, 1, 12, 4),
		[],
		6,
		20,
		40,
		[],
		[]);

	public static ActorType Deathbot { get; } = new(
		"deathbot",
		'R',
		Color.Gray,
		new(16, 2, 5, 16, 5),
		[],
		7,
		30,
		50,
		[Element.Fire, Element.Ice, Element.Air],
		[]);

	public static ActorType ReaperDrone { get; } = new(
		"reaper drone",
		'R',
		Color.White,
		new(10, 2, 3, 12, 8),
		[Rune.Force, Rune.Fire],
		7,
		40,
		40,
		[Element.Fire, Element.Earth],
		[Element.Ice, Element.Air]);

	public static ActorType ChaosMinion { get; } = new(
		"chaos minion",
		'X',
		Color.Purple,
		new(12, 12, 1, 8, 10),
		[Rune.Force, Rune.Fire, Rune.Ice, Rune.Air, Rune.Earth],
		8,
		60,
		30,
		[],
		[]);

	public static ActorType SoulCollector { get; } = new(
		"soul collector",
		'X',
		Color.White,
		new(5, 15, 12, 12, 5),
		[Rune.Force, Rune.Fire, Rune.Ice, Rune.Air, Rune.Earth],
		8,
		80,
		10,
		[],
		[]);

	public static IEnumerable<ActorType> Enemies { get; } =
		[Blob, Hedgehog, Imp, Slug, IceGolem, EarthGolem, AeolianVortex, Ninja,
		DoomFungus, Xenosphere, MadSorcerer, Butcher, Deathbot, ReaperDrone, ChaosMinion, SoulCollector];

	public static ActorType IncineratorOfSouls { get; } = new(
		"Incinerator of Souls",
		'Ω',
		Element.Fire.Color,
		new(8, 20, 10, 10, 6),
		[Rune.Force, Rune.Fire, Rune.Air],
		8,
		100,
		0,
		[Element.Fire, Element.Air],
		[Element.Ice]);

	public static ActorType FrigidPrince { get; } = new(
		"Frigid Prince",
		'Ω',
		Element.Ice.Color,
		new(6, 15, 12, 15, 5),
		[Rune.Force, Rune.Ice, Rune.Earth],
		8,
		100,
		0,
		[Element.Ice, Element.Earth],
		[Element.Fire]);

	public static ActorType LordOfTheGales { get; } = new(
		"Lord of the Gales",
		'Ω',
		Element.Air.Color,
		new(6, 15, 8, 8, 10),
		[Rune.Force, Rune.Air, Rune.Ice],
		8,
		100,
		0,
		[Element.Air, Element.Ice],
		[Element.Earth]);

	public static ActorType TerraCrusher { get; } = new(
		"Terra Crusher",
		'Ω',
		Element.Earth.Color,
		new(10, 10, 10, 20,4),
		[Rune.Force, Rune.Earth, Rune.Fire],
		8,
		100,
		0,
		[Element.Earth, Element.Fire],
		[Element.Air]);

	public static ActorType MasterOfTheFist { get; } = new(
		"Master of the Fist",
		'Ω',
		Element.Force.Color,
		new(15, 6, 8, 15, 7),
		[Rune.Force, Rune.Force],
		8,
		100,
		0,
		[Element.Force],
		[]);

	public static ActorType MageOfAllHues { get; } = new(
		"Mage of All Hues",
		'Ω',
		Color.Gold,
		new(5, 15, 20, 8, 5),
		[Rune.Fire, Rune.Ice, Rune.Air, Rune.Earth],
		8,
		100,
		0,
		[Element.Fire, Element.Ice, Element.Air, Element.Earth],
		[Element.Force]);

	public static ActorType Spaceweaver { get; } = new(
		"Spaceweaver",
		'Ω',
		Color.Purple,
		new(6, 15, 15, 12, 6),
		[Rune.Force, Rune.Warp, Rune.Air],
		8,
		100,
		0,
		[Element.Force, Element.Air],
		[]);

	public static IEnumerable<ActorType> Bosses { get; } =
		[IncineratorOfSouls, FrigidPrince, LordOfTheGales, TerraCrusher, MasterOfTheFist, MageOfAllHues, Spaceweaver];
}
