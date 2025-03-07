using System.Drawing;
using Microsoft.AspNetCore.Components;
using Spellslinger.Models;

namespace Spellslinger.Components;

public partial class DualMeterBar
{
	[Parameter]
	public DualMeter Meter { get; set; }

	[Parameter]
	public int InnerValue { get; set; }

	[Parameter]
	public int InnerMax { get; set; }

	[Parameter]
	public int OuterValue { get; set; }

	[Parameter]
	public int OuterMax { get; set; }

	[Parameter]
	public Color Color { get; set; }

	[Parameter]
	public int InnerWidth { get; set; }

	[Parameter]
	public int OuterWidth { get; set; }

	public int Max => InnerMax + OuterMax;

	protected override void OnInitialized()
	{
		base.OnInitialized();
		Refresh();
	}

	protected override void GameUpdated(object sender, GameUpdatedEventArgs e)
	{
		base.GameUpdated(sender, e);
		if (e.IsGlobal || e.Tile is not null && (e.Tile?.Actor?.HP == Meter || e.Tile?.Actor?.MP == Meter))
		{
			Refresh();
		}
	}

	private void Refresh()
	{
		InnerValue = Meter.Inner.Value;
		InnerMax = Meter.Inner.Maximum == 0 ? 1 : Meter.Inner.Maximum;
		OuterValue = Meter.Outer.Value;
		OuterMax = Meter.Outer.Maximum == 0 ? 1 : Meter.Outer.Maximum;
		InnerWidth = 100 * InnerValue / Max;
		OuterWidth = 100 * OuterValue / Max;
		StateHasChanged();
	}
}
