using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class AdjascentWallComponent : Node
	{
		[Export]
		private RayCast3D BuildableChecker = null;
		[Export]
		private Node3D DefaultWall = null;
		[Export]
		private Node3D WallContainer = null;

		private BuildableDb Buildables = null;
		private BuildableWrapper CurrentBuildable = null;
		private Node3D CurrentBuildableWall = null;

		private void HandleChangeBuildable(BuildableWrapper buildable)
		{
			CurrentBuildableWall?.QueueFree();
			CurrentBuildable = buildable;

			if (buildable == null)
			{
				CurrentBuildableWall = null;
				DefaultWall.Show();
				DefaultWall.ProcessMode = ProcessModeEnum.Inherit;
				return;
			}

			Buildable targetBuildable = Buildables.GetById(buildable.BuildableId);
			if (targetBuildable == null)
			{
				CurrentBuildableWall = null;
				DefaultWall.Show();
				DefaultWall.ProcessMode = ProcessModeEnum.Inherit;
				return;
			}

			if (targetBuildable.AdjascentWall == null)
			{
				CurrentBuildableWall = null;
				DefaultWall.Show();
				DefaultWall.ProcessMode = ProcessModeEnum.Inherit;
				return;
			}

			DefaultWall.Hide();
			DefaultWall.ProcessMode = ProcessModeEnum.Disabled;
			CurrentBuildableWall = targetBuildable.AdjascentWall.Instantiate<Node3D>();
			WallContainer.AddChild(CurrentBuildableWall);
		}

		public override void _Ready()
		{
			base._Ready();
			Buildables = GetNode<BuildableDb>("/root/Buildables");
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			if (BuildableChecker.GetCollider() is BuildableWrapper buildable)
			{
				Vector3 buildableForward = buildable.GlobalBasis.Z.Normalized();
				Vector3 checkerForward = BuildableChecker.TargetPosition.Normalized().Rotated(Vector3.Up, BuildableChecker.GlobalRotation.Y);

				if (!buildableForward.IsEqualApprox(checkerForward))
				{
					if (CurrentBuildable != null)
					{
						HandleChangeBuildable(null);
					}

					return;
				}

				if (CurrentBuildable != buildable)
				{
					HandleChangeBuildable(buildable);
				}
			}
			else if (CurrentBuildable != null)
			{
				HandleChangeBuildable(null);
			}
		}
	}
}