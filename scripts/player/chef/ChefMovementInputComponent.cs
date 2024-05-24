using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class ChefMovementInputComponent : Node
	{
		[Export]
		private ChefMovementComponent Movement = null;
		[Export]
		private Timer MovementCooldown = null;

		private InputInstance InputInstance = null;

		public override void _Ready()
		{
			base._Ready();
			InputInstance = Input.Singleton;
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			if (!MovementCooldown.IsStopped())
			{
				return;
			}

			if (InputInstance.GetActionStrength("ChefMoveForward") >= 0.5f)
			{
				Movement.MoveForward();
				MovementCooldown.Start();
			}
			else if (InputInstance.GetActionStrength("ChefMoveBackward") >= 0.5f)
			{
				Movement.MoveBackward();
				MovementCooldown.Start();
			}
			else if (InputInstance.GetActionStrength("ChefRotateLeft") >= 0.5f)
			{
				Movement.RotateLeft();
				MovementCooldown.Start();
			}
			else if (InputInstance.GetActionStrength("ChefRotateRight") >= 0.5f)
			{
				Movement.RotateRight();
				MovementCooldown.Start();
			}
		}
	}
}
