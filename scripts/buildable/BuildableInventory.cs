using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildableInventory : Resource
	{
		[Export]
		public Array<BuildableInventorySlot> Slots { get; set; } = new();
	}
}
