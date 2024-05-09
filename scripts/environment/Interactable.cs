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
		public delegate void InteractedEventHandler();

		public void Interact()
		{
			EmitSignal(SignalName.Interacted);
		}
	}
}
