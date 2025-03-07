using Microsoft.AspNetCore.Components;
using Spellslinger.Models;
using Spellslinger.Services;

namespace Spellslinger.Components;

public partial class TrainingView
{
	public int StrengthCost => (Game.Player.Stats.Strength + 1) * Game.TrainingCostFactor;

	public int WillpowerCost => (Game.Player.Stats.Willpower + 1) * Game.TrainingCostFactor;

	public int MemoryCost => (Game.Player.Stats.Memory + 1) * Game.TrainingCostFactor;

	public int ToughnessCost => (Game.Player.Stats.Toughness + 1) * Game.TrainingCostFactor;

	public int SpeedCost => (Game.Player.Stats.Speed + 1) * Game.TrainingCostFactor;

	public void TrainStrength()
	{
		if (Game.Player.Experience >= StrengthCost)
		{
			Game.Player.Experience -= StrengthCost;
			Game.Player.Stats = Game.Player.Stats with { Strength = Game.Player.Stats.Strength + 1 };
		}
		HealPlayer();
	}

	public void TrainWillpower()
	{
		if (Game.Player.Experience >= WillpowerCost)
		{
			Game.Player.Experience -= WillpowerCost;
			Game.Player.Stats = Game.Player.Stats with { Willpower = Game.Player.Stats.Willpower + 1 };
		}
		HealPlayer();
	}

	public void TrainMemory()
	{
		if (Game.Player.Experience >= MemoryCost)
		{
			Game.Player.Experience -= MemoryCost;
			Game.Player.Stats = Game.Player.Stats with { Memory = Game.Player.Stats.Memory + 1 };
		}
		HealPlayer();
	}

	public void TrainToughness()
	{
		if (Game.Player.Experience >= ToughnessCost)
		{
			Game.Player.Experience -= ToughnessCost;
			Game.Player.Stats = Game.Player.Stats with { Toughness = Game.Player.Stats.Toughness + 1 };
		}
		HealPlayer();
	}

	public void TrainSpeed()
	{
		if (Game.Player.Experience >= SpeedCost)
		{
			Game.Player.Experience -= SpeedCost;
			Game.Player.Stats = Game.Player.Stats with { Speed = Game.Player.Stats.Speed + 1 };
		}
		HealPlayer();
	}

	private void ReturnToTown()
	{
		Game.InputMode = InputMode.Town;
	}

	private void HealPlayer()
	{
		Game.Player.SetupStats();
		Game.Player.HP.Restore();
		Game.Player.MP.Restore();
		Game.Update();
	}
}
