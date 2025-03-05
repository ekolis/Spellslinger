using System.Drawing;
using Microsoft.AspNetCore.Components;
using Spellslinger.Models;

namespace Spellslinger.Components;

public partial class DualMeterBar
{
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
		InnerWidth = 100 * InnerValue / Max;
		OuterWidth = 100 * OuterValue / Max;
	}
}
