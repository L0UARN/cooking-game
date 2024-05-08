using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class ChefMovementComponent : Node
	{
		[Export]
		private GridNavigatorComponent Navigator = null;
		[Export]
		private Node3D Body = null;

		[Signal]
		public delegate void MovedEventHandler();

		public void MoveForward()
		{
			if (Navigator.NavigateUp())
			{
				EmitSignal(SignalName.Moved);
			}
		}

		public void MoveBackward()
		{
			if (Navigator.NavigateDown())
			{
				EmitSignal(SignalName.Moved);
			}
		}

		public void RotateLeft()
		{
			Body.Rotate(Vector3.Up, Mathf.Pi / 2);
			EmitSignal(SignalName.Moved);
		}

		public void RotateRight()
		{
			Body.Rotate(Vector3.Up, -Mathf.Pi / 2);
			EmitSignal(SignalName.Moved);
		}
	}
}
