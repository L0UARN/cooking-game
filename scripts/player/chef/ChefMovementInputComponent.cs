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

			if (InputInstance.GetActionStrength("MoveForward") >= 0.5f)
			{
				Movement.MoveForward();
				MovementCooldown.Start();
			}
			else if (InputInstance.GetActionStrength("MoveBackward") >= 0.5f)
			{
				Movement.MoveBackward();
				MovementCooldown.Start();
			}
			else if (InputInstance.GetActionStrength("RotateLeft") >= 0.5f)
			{
				Movement.RotateLeft();
				MovementCooldown.Start();
			}
			else if (InputInstance.GetActionStrength("RotateRight") >= 0.5f)
			{
				Movement.RotateRight();
				MovementCooldown.Start();
			}
		}
	}
}
