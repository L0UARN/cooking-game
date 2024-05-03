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

		public void MoveForward()
		{
			Navigator.NavigateUp();
		}

		public void MoveBackward()
		{
			Navigator.NavigateDown();
		}

		public void RotateLeft()
		{
			Body.Rotate(Vector3.Up, Mathf.Pi / 2);
		}

		public void RotateRight()
		{
			Body.Rotate(Vector3.Up, -Mathf.Pi / 2);
		}
	}
}
