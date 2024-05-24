using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuilderInventoryInputComponent : Node
	{
		[Export]
		private BuilderInventoryComponent Inventory = null;
		[Export]
		private Timer CycleCooldown = null;
		[Export]
		private Timer HoldCycleDelay = null;
		[Export]
		private Timer FastCycleCooldown = null;

		private InputInstance InputInstance = null;
		private bool IsHoldingCycle = false;

		public override void _Ready()
		{
			base._Ready();
			InputInstance = Input.Singleton;
		}

		private void HandleCycleHoldDetection()
		{
			if (InputInstance.IsActionJustPressed("BuilderInventoryCycleNext") || InputInstance.IsActionJustPressed("BuilderInventoryCyclePrevious"))
			{
				HoldCycleDelay.Start();
				IsHoldingCycle = true;
			}
			else if (InputInstance.IsActionJustReleased("BuilderInventoryCycleNext") || InputInstance.IsActionJustReleased("BuilderInventoryCyclePrevious"))
			{
				HoldCycleDelay.Stop();
				IsHoldingCycle = false;
			}
		}

		private void HandleCycling()
		{
			Timer cooldown = CycleCooldown;
			if (IsHoldingCycle && HoldCycleDelay.IsStopped())
			{
				cooldown = FastCycleCooldown;
			}

			if (!cooldown.IsStopped())
			{
				return;
			}

			if (InputInstance.GetActionStrength("BuilderInventoryCycleNext") >= 0.5f)
			{
				Inventory.CycleNextAvailable();
				cooldown.Start();
			}
			else if (InputInstance.GetActionStrength("BuilderInventoryCyclePrevious") >= 0.5f)
			{
				Inventory.CyclePreviousAvailable();
				cooldown.Start();
			}
		}

		private void HandleSwitching()
		{
			if (InputInstance.IsActionPressed("BuilderInventorySlot1"))
			{
				Inventory.Selection = Mathf.Min(0, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("BuilderInventorySlot2"))
			{
				Inventory.Selection = Mathf.Min(1, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("BuilderInventorySlot3"))
			{
				Inventory.Selection = Mathf.Min(2, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("BuilderInventorySlot4"))
			{
				Inventory.Selection = Mathf.Min(3, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("BuilderInventorySlot5"))
			{
				Inventory.Selection = Mathf.Min(4, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("BuilderInventorySlot6"))
			{
				Inventory.Selection = Mathf.Min(5, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("BuilderInventorySlot7"))
			{
				Inventory.Selection = Mathf.Min(6, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("BuilderInventorySlot8"))
			{
				Inventory.Selection = Mathf.Min(7, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("BuilderInventorySlot9"))
			{
				Inventory.Selection = Mathf.Min(8, Inventory.Slots.Count - 1);
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			HandleCycleHoldDetection();
			HandleCycling();
			HandleSwitching();
		}
	}
}
