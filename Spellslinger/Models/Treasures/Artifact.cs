using System.Drawing;
using Spellslinger.Services;

namespace Spellslinger.Models.Treasures;

/// <summary>
/// The artifact that you need to collect and remove from the dungeon to win the game.
/// </summary>
public class Artifact
	: Treasure
{
	public Artifact(IGame game)
		: base(game)
	{ 
	}

	public override string Name => "Orb of MacGuffin";
	public override char Character => '0';
	public override Color Color => Color.Gold;
	public override int Priority => 0;

	public override void OnCollect(Map map, int x, int y, Actor actor)
	{
		if (actor.IsPlayerControlled)
		{
			// take note that the player has collected the artifact
			Game.IsArtifactCollected = true;

			// remove the artifact from the floor
			map.Tiles[x, y].Treasures.Remove(this);

			// display a message
			Game.Log.Add($"You collect the {this}! Now escape from the dungeon to seal away the evil once and for all!");

			// TODO: awaken the hibernating boss
		}
	}

	public override void OnSpawn(Map map, int x, int y)
	{
		// TODO: spawn a hibernating boss enemy
	}
}
