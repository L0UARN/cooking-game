using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class ItemContainerComponent : Node
	{
		[Export]
		private Interactable Interactable = null;
		[Export]
		private ItemDisplay ItemDisplay = null;

		private void HandleInteract(ChefInventoryComponent inventory, StringName slot)
		{
			inventory.SwapItem(ItemDisplay.Item, slot);
		}

		public override void _Ready()
		{
			base._Ready();
			Interactable.Interacted += HandleInteract;
		}
	}
}
