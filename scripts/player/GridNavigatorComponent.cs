using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class GridNavigatorComponent : Node
	{
		[Export]
		private Node3D Body = null;
		[Export]
		private RayCast3D NextTileChecker = null;
		[Export]
		private RayCast3D NextCollisionChecker = null;
		[Export]
		private RayCast3D CurrentTileChecker = null;

		[Signal]
		public delegate void NavigatedEventHandler();

		private bool ShouldDoInitialCheck = true;
		private BuildableDb Buildables = null;

		public GridTile GetHoveredTile()
		{
			CurrentTileChecker.ProcessMode = ProcessModeEnum.Inherit;
			CurrentTileChecker.ForceRaycastUpdate();
			CurrentTileChecker.ProcessMode = ProcessModeEnum.Disabled;

			if (CurrentTileChecker.GetCollider() is GridTile tile)
			{
				return tile;
			}

			return null;
		}

		public GridTile GetTileInDirection(float angle)
		{
			if (NextCollisionChecker != null)
			{
				NextCollisionChecker.Basis = Basis.Identity;
				NextCollisionChecker.Rotate(Vector3.Up, angle);

				NextCollisionChecker.ProcessMode = ProcessModeEnum.Inherit;
				NextCollisionChecker.ForceRaycastUpdate();
				NextCollisionChecker.ProcessMode = ProcessModeEnum.Disabled;

				if (NextCollisionChecker.GetCollider() is BuildableComponent buildable)
				{
					Buildable targetBuildable = Buildables.GetById(buildable.BuildableId);
					if (targetBuildable != null && targetBuildable.IsSolid)
					{
						return null;
					}
				}
			}

			NextTileChecker.Basis = Basis.Identity;
			NextTileChecker.Rotate(Vector3.Up, angle);

			NextTileChecker.ProcessMode = ProcessModeEnum.Inherit;
			NextTileChecker.ForceRaycastUpdate();
			NextTileChecker.ProcessMode = ProcessModeEnum.Disabled;

			if (NextTileChecker.GetCollider() is GridTile area)
			{
				return area;
			}

			return null;
		}

		public void NavigateToTile(GridTile tile)
		{
			if (tile.Placement.GlobalPosition.IsEqualApprox(Body.GlobalPosition))
			{
				return;
			}

			Body.GlobalPosition = tile.Placement.GlobalPosition;
			EmitSignal(SignalName.Navigated);
		}

		public bool NavigateUp()
		{
			GridTile target = GetTileInDirection(0.0f);
			if (target == null)
			{
				return false;
			}

			NavigateToTile(target);
			return true;
		}

		public bool NavigateDown()
		{
			GridTile target = GetTileInDirection(Mathf.Pi);
			if (target == null)
			{
				return false;
			}

			NavigateToTile(target);
			return true;
		}

		public bool NavigateLeft()
		{
			GridTile target = GetTileInDirection(Mathf.Pi * 0.5f);
			if (target == null)
			{
				return false;
			}

			NavigateToTile(target);
			return true;
		}

		public bool NavigateRight()
		{
			GridTile target = GetTileInDirection(Mathf.Pi * 1.5f);
			if (target == null)
			{
				return false;
			}

			NavigateToTile(target);
			return true;
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			if (!ShouldDoInitialCheck)
			{
				return;
			}

			GridTile hoveredTile = GetHoveredTile();
			if (hoveredTile != null)
			{
				NavigateToTile(hoveredTile);
			}

			ShouldDoInitialCheck = false;
		}

		public override void _Ready()
		{
			NextTileChecker.ProcessMode = ProcessModeEnum.Disabled;
			CurrentTileChecker.ProcessMode = ProcessModeEnum.Disabled;

			if (NextCollisionChecker != null)
			{
				NextCollisionChecker.ProcessMode = ProcessModeEnum.Disabled;
				Buildables = GetNode<BuildableDb>("/root/Buildables");
			}
		}
	}
}
