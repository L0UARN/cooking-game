using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuilderInventoryComponent : Node
	{
		[Export]
		private BuildableInventory Inventory = null;

		[Signal]
		public delegate void SelectionChangedEventHandler(int newSelection);
		[Signal]
		public delegate void SlotChangedEventHandler(int slotIndex, BuildableInventorySlot slot);

		private int _Selection = 0;
		public int Selection
		{
			get => _Selection;
			set
			{
				_Selection = Mathf.Abs(value % Inventory.Slots.Count);
				EmitSignal(SignalName.SelectionChanged, _Selection);
			}
		}

		public void Add(StringName buildableId, int quantity)
		{
			for (int i = 0; i < Inventory.Slots.Count; i++)
			{
				if (Inventory.Slots[i].Buildable.Id == buildableId)
				{
					Inventory.Slots[i].Quantity += quantity;
					EmitSignal(SignalName.SlotChanged, i, Inventory.Slots[i]);
					return;
				}
			}
		}

		public void Remove(StringName buildableId, int quantity)
		{
			for (int i = 0; i < Inventory.Slots.Count; i++)
			{
				if (Inventory.Slots[i].Buildable.Id == buildableId)
				{
					Inventory.Slots[i].Quantity -= quantity;
					EmitSignal(SignalName.SlotChanged, i, Inventory.Slots[i]);

					if (Selection == i && SelectedSlot.Quantity == 0)
					{
						CycleNextAvailable();
					}

					return;
				}
			}
		}

		public void CycleNextAvailable()
		{
			int newSelection = Selection;
			for (int i = 0; i < Inventory.Slots.Count; i++)
			{
				newSelection = Mathf.Abs((newSelection + 1) % Inventory.Slots.Count);
				if (Inventory.Slots[newSelection].Quantity > 0)
				{
					Selection = newSelection;
					return;
				}
			}
		}

		public void CyclePreviousAvailable()
		{
			int newSelection = Selection;
			for (int i = 0; i < Inventory.Slots.Count; i++)
			{
				newSelection = Mathf.Abs((newSelection - 1) % Inventory.Slots.Count);
				if (Inventory.Slots[newSelection].Quantity > 0)
				{
					Selection = newSelection;
					return;
				}
			}
		}

		public BuildableInventorySlot SelectedSlot => Inventory.Slots[_Selection];
		public Buildable SelectedBuildable => SelectedSlot.Buildable;
		public Array<BuildableInventorySlot> Slots => Inventory.Slots;
	}
}
