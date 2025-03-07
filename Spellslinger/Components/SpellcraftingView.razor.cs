using Microsoft.AspNetCore.Components;
using Spellslinger.Models;

namespace Spellslinger.Components;

public partial class SpellcraftingView
{
	/// <summary>
	/// Runes which are currently being used to craft a spell.
	/// </summary>
	[Parameter]
	public IList<Rune> CraftingRunes { get; set; } = [];

	/// <summary>
	/// The spell that is currently being crafted, if any.
	/// </summary>
	[Parameter]
	public Spell? CraftingSpell { get; set; }

	/// <summary>
	/// The spell that is currently being examined, if any.
	/// </summary>
	[Parameter]
	public Spell? ExaminedSpell { get; set; }

	private void AddRune(Rune rune)
	{
		ExaminedSpell = null;
		CraftingRunes.Add(rune);
		if (CraftingSpell is null)
		{
			// crafting a new spell
			CraftingSpell = rune.Spell;
		}
		else
		{
			// adding a modifier rune
			CraftingSpell = CraftingSpell.ApplyModifier(rune.Modifier);
		}
		StateHasChanged();
	}

	private void RemoveRune(Rune rune)
	{
		CraftingRunes.Remove(rune);

		if (CraftingRunes.Any())
		{
			if (CraftingRunes.First().Spell is null)
			{
				// first rune no longer has a spell, just clear them all because the spell is invalid
				CraftingRunes.Clear();
				CraftingSpell = null;
			}
			else
			{
				// rebuild the spell wih the remaining runes
				CraftingSpell = CraftingRunes.First().Spell;
				foreach (var modifierRune in CraftingRunes.Skip(1))
				{
					CraftingSpell = CraftingSpell.ApplyModifier(modifierRune.Modifier);
				}
			}
		}
		else
		{
			// clear the spell, it has no runes
			CraftingSpell = null;
		}
	}

	private void LearnCraftedSpell()
	{
		// TODO: don't allow duplicate spells to be learned (will require tracking which runes make up which spells)
		if (CraftingSpell is not null)
		{
			Game.Player?.Knowledge.Spells.Add(CraftingSpell);
			CraftingSpell = null;
			CraftingRunes.Clear();
		}
	}

	private void ExamineSpell(Spell spell)
	{
		CraftingRunes.Clear();
		CraftingSpell = null;
		ExaminedSpell = spell;
	}

	private void ReturnToTown()
	{
		Game.InputMode = InputMode.Town;
	}
}
