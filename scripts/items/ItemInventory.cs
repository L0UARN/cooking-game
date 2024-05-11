using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class ItemInventory : Resource
	{
		// Sadly Godot doesn't support enums as exported dictionnary keys, so we have to use constants instead
		public readonly static StringName SlotLeftHand = "LeftHand";
		public readonly static StringName SlotRightHand = "RightHand";

		// Exported dictionnaries are terrible to work with in editor, but that's the best we can do (I think)
		[Export]
		public Dictionary<StringName, Item> Items { get; set; } = new();
	}
}
