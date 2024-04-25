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
		private RayCast3D CurrentTileChecker = null;
		private bool ShouldDoInitialCheck = true;

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
			NextTileChecker.ForceRaycastUpdate();

			if (NextTileChecker.GetCollider() is GridTile area)
			{
				return area;
			}
			else
			{
				return null;
			}
		}

		private bool MoveToTarget()
		{
			GridTile target = GetTarget();
			if (target == null)
			{
				return false;
			}

			Body.GlobalPosition = target.Placement.GlobalPosition;
			return true;
		}

		public GridTile GetTile()
		{
			if (CurrentTileChecker.GetCollider() is GridTile tile)
			{
				return tile;
			}

			return null;
		}

		public bool NavigateUp()
		{
			if (NextTileChecker == null)
			{
				return false;
			}

			NextTileChecker.Basis = Basis.Identity;
			return MoveToTarget();
		}

		public bool NavigateDown()
		{
			if (NextTileChecker == null)
			{
				return false;
			}

			NextTileChecker.Basis = Basis.Identity;
			NextTileChecker.RotateY(Mathf.Pi);
			return MoveToTarget();
		}

		public bool NavigateLeft()
		{
			if (NextTileChecker == null)
			{
				return false;
			}

			NextTileChecker.Basis = Basis.Identity;
			NextTileChecker.RotateY(Mathf.Pi / 2);
			return MoveToTarget();
		}

		public bool NavigateRight()
		{
			if (NextTileChecker == null)
			{
				return false;
			}

			NextTileChecker.Basis = Basis.Identity;
			NextTileChecker.RotateY(-Mathf.Pi / 2);
			return MoveToTarget();
		}

		public void MoveTo(GridTile tile)
		{
			Body.GlobalPosition = tile.Placement.GlobalPosition;
		}
	}
}
