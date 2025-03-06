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

			// awaken the boss
			Game.Boss.Awaken();
		}
	}

	public override void OnSpawn(Map map, int x, int y)
	{
		// spawn the boss nearby
		var found = false;
		var distance = 1;
		while (!found && distance <= map.Width + map.Height - 2)
		{
			var candidates = new List<(int x, int y)>();
			for (var cx = 0; cx < map.Width; cx++)
			{
				for (var cy = 0; cy < map.Height; cy++)
				{
					if (Math.Abs(cx - x) + Math.Abs(cy - y) == distance && map.Tiles[x, y].Actor is null)
					{
						candidates.Add((cx, cy));
					}
				}
			}
			if (candidates.Any())
			{
				var candidate = candidates.PickWeighted(q => 1, Game.Rng);
				map.Tiles[candidate.x, candidate.y].Actor = Game.Boss;
				found = true;
			}
		}
	}
}
