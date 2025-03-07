using Spellslinger.Services;

namespace Spellslinger.Models.Spells;

public record Ascend
	: Spell
{
	public Ascend()
	{
		Stats = new SpellStats(
			Name: x => $"Ascend",
			Description: x => $"Ascends one dungeon level.",
			Details: x => "You will be placed on the down stairs.",
			MPCost: 12,
			Element: Element.Air,
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
			game.Log.Add($"The {caster} floats up through the ceiling!");
			if (caster.IsPlayerControlled)
			{
				caster.PlayerAscend();
			}
			else
			{
				caster.Tile = null;
			}
		}
	}
}
