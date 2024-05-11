using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class Interactable : Area3D
	{
		[Export]
		public Interactable Up { get; set; } = null;
		[Export]
		public Interactable Down { get; set; } = null;
		[Export]
		public Interactable Left { get; set; } = null;
		[Export]
		public Interactable Right { get; set; } = null;

		[Signal]
		public delegate void InteractedEventHandler(ChefInventoryComponent inventory, StringName slot);

		public void Interact(ChefInventoryComponent inventory, StringName slot)
		{
			EmitSignal(SignalName.Interacted, inventory, slot);
		}
	}
}
