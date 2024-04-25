using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuilderInventoryUi : CanvasLayer
	{
		[Export]
		private BuilderInventoryComponent Inventory = null;
		[Export]
		private Control SlotContainer = null;
		[Export]
		private PackedScene SlotScene = null;

		private Array<BuilderInventorySlotUi> Slots = new();

		public override void _Ready()
		{
			base._Ready();

			for (int i = 0; i < Inventory.Slots.Count; i++)
			{
				BuilderInventorySlotUi slotUi = SlotScene.Instantiate<BuilderInventorySlotUi>();
				slotUi.Slot = Inventory.Slots[i];
				slotUi.Selected = i == Inventory.Selection;
				Slots.Add(slotUi);
				SlotContainer.AddChild(slotUi);
			}

			Inventory.SlotChanged += HandleSlotChanged;
			Inventory.SelectionChanged += HandleSelectionChanged;
		}

		private void HandleSlotChanged(int slotIndex, BuildableInventorySlot slot)
		{
			Slots[slotIndex].Slot = slot;
		}

		private void HandleSelectionChanged(int newSelection)
		{
			for (int i = 0; i < Slots.Count; i++)
			{
				Slots[i].Selected = i == newSelection;
			}
		}
	}
}
