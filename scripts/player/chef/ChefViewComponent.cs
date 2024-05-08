using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class ChefViewComponent : Node
	{
		[Export]
		private Node3D Head = null;
		[Export]
		private RayCast3D PointOfViewChecker = null;
		[Export]
		private ChefMovementComponent Movement = null;

		private void HandleView()
		{
			PointOfViewChecker.ProcessMode = ProcessModeEnum.Inherit;
			PointOfViewChecker.ForceRaycastUpdate();
			PointOfViewChecker.ProcessMode = ProcessModeEnum.Disabled;

			if (PointOfViewChecker.GetCollider() is PointOfView pointOfView)
			{
				Head.GlobalTransform = pointOfView.Placement.GlobalTransform;
			}
			else
			{
				Head.Transform = Transform3D.Identity;
			}
		}

		public override void _Ready()
		{
			base._Ready();

			PointOfViewChecker.ProcessMode = ProcessModeEnum.Disabled;
			Movement.Moved += HandleView;
		}
	}
}
