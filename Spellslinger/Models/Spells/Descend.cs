using Spellslinger.Services;

namespace Spellslinger.Models.Spells;

public record Descend
	: Spell
{
	public Descend()
	{
		Stats = new SpellStats(
			Name: x => $"Descend",
			Description: x => $"Descends one dungeon level.",
			Details: x => "You will be placed on the up stairs.",
			MPCost: 8,
			Element: Element.Earth,
			Power: x => 0,
			Knockback: x => 0,
			Teleport: x => 0,
			Range: x => 0);
	}

	public override bool IsDirectional => false;

	protected override void CastImpl(IGame game, Actor caster, int dx, int dy)
	{
		if (game.IsArtifactCollected)
		{
			game.Log.Add("The Orb of MacGuffin's mystical power prevents the spell from working!");
		}
		else
		{
			game.Log.Add($"The {caster} drills down through the floor!");
			if (caster.IsPlayerControlled)
			{
				caster.PlayerDescend();
			}
			else
			{
				caster.Tile = null;
			}
		}
	}
}
