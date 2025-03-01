using Microsoft.AspNetCore.Components;

namespace Spellslinger.Components;

/// <summary>
/// A Razor component that has a view model.
/// </summary>
public class ModelComponentBase<T>
	: ComponentBase
{
	/// <summary>
	/// The view model.
	/// </summary>
	[Parameter]
	public required T Model { get; set; }
}
