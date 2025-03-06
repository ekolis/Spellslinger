using Microsoft.AspNetCore.Components;
using Spellslinger.Models;
using Spellslinger.Services;

namespace Spellslinger.Components;

public partial class TrainingView
{
	public int StrengthCost => (int)Math.Pow(Game.Player.Stats.Strength + 1, 2) * Game.TrainingCostFactor;

	public int WillpowerCost => (int)Math.Pow(Game.Player.Stats.Willpower + 1, 2) * Game.TrainingCostFactor;

	public int MemoryCost => (int)Math.Pow(Game.Player.Stats.Memory + 1, 2) * Game.TrainingCostFactor;

	public int ToughnessCost => (int)Math.Pow(Game.Player.Stats.Toughness + 1, 2) * Game.TrainingCostFactor;

	public int SpeedCost => (int)Math.Pow(Game.Player.Stats.Speed + 1, 2) * Game.TrainingCostFactor;

	public void TrainStrength()
	{
		if (Game.Player.Experience >= StrengthCost)
		{
			Game.Player.Experience -= StrengthCost;
			Game.Player.Stats = Game.Player.Stats with { Strength = Game.Player.Stats.Strength + 1 };
		}
	}

	public void TrainWillpower()
	{
		if (Game.Player.Experience >= WillpowerCost)
		{
			Game.Player.Experience -= WillpowerCost;
			Game.Player.Stats = Game.Player.Stats with { Willpower = Game.Player.Stats.Willpower + 1 };
		}
	}

	public void TrainMemory()
	{
		if (Game.Player.Experience >= MemoryCost)
		{
			Game.Player.Experience -= MemoryCost;
			Game.Player.Stats = Game.Player.Stats with { Memory = Game.Player.Stats.Memory + 1 };
		}
	}

	public void TrainToughness()
	{
		if (Game.Player.Experience >= ToughnessCost)
		{
			Game.Player.Experience -= ToughnessCost;
			Game.Player.Stats = Game.Player.Stats with { Toughness = Game.Player.Stats.Toughness + 1 };
		}
	}

	public void TrainSpeed()
	{
		if (Game.Player.Experience >= SpeedCost)
		{
			Game.Player.Experience -= SpeedCost;
			Game.Player.Stats = Game.Player.Stats with { Speed = Game.Player.Stats.Speed + 1 };
		}
	}

	private void ReturnToTown()
	{
		Game.InputMode = InputMode.Town;
	}
}
