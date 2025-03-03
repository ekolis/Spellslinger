namespace Spellslinger.Models.SpellModifiers;

public class Force
	: SpellModifier
{
	public override string Description => "Increases the damage inflicted by a spell.";
	public override string Details => "Damage increase scales with strength.";
	public override int MPCost => 4;
}
