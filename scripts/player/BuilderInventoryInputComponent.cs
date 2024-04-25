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
			if (InputInstance.IsActionJustPressed("CycleNext") || InputInstance.IsActionJustPressed("CyclePrevious"))
			{
				HoldCycleDelay.Start();
				IsHoldingCycle = true;
			}
			else if (InputInstance.IsActionJustReleased("CycleNext") || InputInstance.IsActionJustReleased("CyclePrevious"))
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

			if (InputInstance.GetActionStrength("CycleNext") >= 0.5f)
			{
				Inventory.CycleNextAvailable();
				cooldown.Start();
			}
			else if (InputInstance.GetActionStrength("CyclePrevious") >= 0.5f)
			{
				Inventory.CyclePreviousAvailable();
				cooldown.Start();
			}
		}

		private void HandleSwitching()
		{
			if (InputInstance.IsActionPressed("Slot1"))
			{
				Inventory.Selection = Mathf.Min(0, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("Slot2"))
			{
				Inventory.Selection = Mathf.Min(1, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("Slot3"))
			{
				Inventory.Selection = Mathf.Min(2, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("Slot4"))
			{
				Inventory.Selection = Mathf.Min(3, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("Slot5"))
			{
				Inventory.Selection = Mathf.Min(4, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("Slot6"))
			{
				Inventory.Selection = Mathf.Min(5, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("Slot7"))
			{
				Inventory.Selection = Mathf.Min(6, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("Slot8"))
			{
				Inventory.Selection = Mathf.Min(7, Inventory.Slots.Count - 1);
			}
			else if (InputInstance.IsActionPressed("Slot9"))
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
