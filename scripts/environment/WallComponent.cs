using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class WallComponent : Node
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
				return;
			}

			Buildable targetBuildable = Buildables.GetById(buildable.BuildableId);
			if (targetBuildable == null)
			{
				CurrentBuildableWall = null;
				DefaultWall.Show();
				return;
			}

			if (targetBuildable.AdjacentWall == null)
			{
				CurrentBuildableWall = null;
				DefaultWall.Show();
				return;
			}

			DefaultWall.Hide();
			CurrentBuildableWall = targetBuildable.AdjacentWall.Instantiate<Node3D>();
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
