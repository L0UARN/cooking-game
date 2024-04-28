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

		private BuildableDb Buildables = null;

		public override void _Ready()
		{
			base._Ready();
			Buildables = GetNode<BuildableDb>("/root/Buildables");
		}

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
				if (Inventory.Slots[i].BuildableId == buildableId)
				{
					Inventory.Slots[i].Quantity += quantity;
					EmitSignal(SignalName.SlotChanged, i, Inventory.Slots[i]);
					return;
				}
			}

			Buildable target = Buildables.GetById(buildableId);
			if (target == null)
			{
				return;
			}

			BuildableInventorySlot newSlot = new() { BuildableId = buildableId, Quantity = quantity };
			Inventory.Slots.Add(newSlot);
		}

		public void Remove(StringName buildableId, int quantity)
		{
			for (int i = 0; i < Inventory.Slots.Count; i++)
			{
				if (Inventory.Slots[i].BuildableId == buildableId)
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
		public StringName SelectedBuildableId => SelectedSlot.BuildableId;
		public Array<BuildableInventorySlot> Slots => Inventory.Slots;
	}
}
