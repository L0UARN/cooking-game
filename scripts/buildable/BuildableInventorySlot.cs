using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildableInventorySlot : Resource
	{
		[Export]
		public StringName BuildableId { get; set; } = null;
		[Export(PropertyHint.Range, "1,99,1")]
		public int Quantity { get; set; } = 1;
	}
}
