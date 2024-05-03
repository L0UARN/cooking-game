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

		public override void _Ready()
		{
			NextTileChecker.ProcessMode = ProcessModeEnum.Disabled;
			CurrentTileChecker.ProcessMode = ProcessModeEnum.Disabled;

			if (NextCollisionChecker != null)
			{
				NextCollisionChecker.ProcessMode = ProcessModeEnum.Disabled;
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			if (!ShouldDoInitialCheck)
			{
				return;
			}

			GridTile currentTile = GetTile();
			if (currentTile != null)
			{
				Body.GlobalPosition = currentTile.Placement.GlobalPosition;
			}

			ShouldDoInitialCheck = false;
		}

		private GridTile GetTarget()
		{
			if (NextCollisionChecker != null)
			{
				NextCollisionChecker.ProcessMode = ProcessModeEnum.Inherit;
				NextCollisionChecker.ForceRaycastUpdate();
				NextCollisionChecker.ProcessMode = ProcessModeEnum.Disabled;

				if (NextCollisionChecker.IsColliding())
				{
					return null;
				}
			}

			NextTileChecker.ProcessMode = ProcessModeEnum.Inherit;
			NextTileChecker.ForceRaycastUpdate();
			NextTileChecker.ProcessMode = ProcessModeEnum.Disabled;

			if (NextTileChecker.GetCollider() is GridTile area)
			{
				return area;
			}

			return null;
		}

		private bool MoveToTarget()
		{
			GridTile target = GetTarget();
			if (target == null)
			{
				return false;
			}

			Body.GlobalPosition = target.Placement.GlobalPosition;
			EmitSignal(SignalName.Navigated);
			return true;
		}

		public GridTile GetTile()
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

		public bool NavigateUp()
		{
			if (NextCollisionChecker != null)
			{
				NextCollisionChecker.Basis = Basis.Identity;
			}

			NextTileChecker.Basis = Basis.Identity;
			return MoveToTarget();
		}

		public bool NavigateDown()
		{
			if (NextCollisionChecker != null)
			{
				NextCollisionChecker.Basis = Basis.Identity;
				NextCollisionChecker.RotateY(Mathf.Pi);
			}

			NextTileChecker.Basis = Basis.Identity;
			NextTileChecker.RotateY(Mathf.Pi);
			return MoveToTarget();
		}

		public bool NavigateLeft()
		{
			if (NextCollisionChecker != null)
			{
				NextCollisionChecker.Basis = Basis.Identity;
				NextCollisionChecker.RotateY(Mathf.Pi / 2);
			}

			NextTileChecker.Basis = Basis.Identity;
			NextTileChecker.RotateY(Mathf.Pi / 2);
			return MoveToTarget();
		}

		public bool NavigateRight()
		{
			if (NextCollisionChecker != null)
			{
				NextCollisionChecker.Basis = Basis.Identity;
				NextCollisionChecker.RotateY(-Mathf.Pi / 2);
			}

			NextTileChecker.Basis = Basis.Identity;
			NextTileChecker.RotateY(-Mathf.Pi / 2);
			return MoveToTarget();
		}

		public void MoveTo(GridTile tile)
		{
			Body.GlobalPosition = tile.Placement.GlobalPosition;
			EmitSignal(SignalName.Navigated);
		}
	}
}
