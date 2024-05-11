using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class ChefInventoryComponent : Node
	{
		[Export]
		private ItemInventory Inventory = null;

		public bool HasItem(StringName slot)
		{
			return Inventory.Items.ContainsKey(slot);
		}

		public Item GetItem(StringName slot)
		{
			return Inventory.Items[slot];
		}

		public void SetItem(Item item, StringName slot)
		{
			Inventory.Items[slot] = item;
		}

		public Item RemoveItem(StringName slot)
		{
			Item item = Inventory.Items[slot];
			Inventory.Items.Remove(slot);
			return item;
		}

		public Item SwapItem(Item item, StringName slot)
		{
			Item oldItem = Inventory.Items[slot];
			Inventory.Items[slot] = item;
			return oldItem;
		}
	}
}
