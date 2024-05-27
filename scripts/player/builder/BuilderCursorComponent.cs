using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuilderCursorComponent : Node
	{
		[Export]
		public BuilderInventoryComponent Inventory { get; set; } = null;
		[Export]
		private GridNavigatorComponent Navigator = null;

		[Export]
		private RayCast3D BuildableChecker = null;
		[Export]
		private RayCast3D WallChecker = null;

		private BuildableDb Buildables = null;
		private BuildableComponent SelectedBuildable = null;

		public BuildableComponent GetHoveredBuildable()
		{
			BuildableChecker.ProcessMode = ProcessModeEnum.Inherit;
			BuildableChecker.ForceRaycastUpdate();
			BuildableChecker.ProcessMode = ProcessModeEnum.Disabled;

			if (BuildableChecker.GetCollider() is BuildableComponent buildable)
			{
				return buildable;
			}

			return null;
		}

		public bool CheckForWall(Vector3 direction)
		{
			WallChecker.ProcessMode = ProcessModeEnum.Inherit;
			WallChecker.TargetPosition = direction;
			WallChecker.ForceRaycastUpdate();
			WallChecker.ProcessMode = ProcessModeEnum.Disabled;

			return WallChecker.IsColliding();
		}

		private void HandleSelectHoveredBuildable()
		{
			BuildableComponent hoveredBuildable = GetHoveredBuildable();

			if (SelectedBuildable == hoveredBuildable)
			{
				return;
			}

			if (hoveredBuildable == null && SelectedBuildable != null)
			{
				SelectedBuildable.Selected = false;
				SelectedBuildable = null;
				return;
			}

			if (SelectedBuildable != null)
			{
				SelectedBuildable.Selected = false;
			}

			SelectedBuildable = hoveredBuildable;
			SelectedBuildable.Selected = true;
		}

		public void Build()
		{
			if (Inventory.SelectedSlot.Quantity <= 0)
			{
				return;
			}

			Buildable buildable = Buildables.GetById(Inventory.SelectedBuildableId);
			if (buildable == null)
			{
				return;
			}

			BuildableComponent hoveredBuildable = GetHoveredBuildable();
			if (hoveredBuildable != null)
			{
				return;
			}

			GridTile currentTile = Navigator.GetTile();
			if (currentTile == null)
			{
				return;
			}

			if (buildable.PlacementStrategy != null && !buildable.PlacementStrategy.Place(buildable, this))
			{
				return;
			}

			Inventory.Remove(Inventory.SelectedBuildableId, 1);
			BuildableWrapper buildableInstance = buildable.Scene.Instantiate<BuildableWrapper>();
			buildableInstance.GlobalTransform = currentTile.GlobalTransform;
			GetTree().Root.AddChild(buildableInstance);
			HandleSelectHoveredBuildable();
		}

		public void Destroy()
		{
			if (SelectedBuildable == null)
			{
				return;
			}

			Buildable buildable = Buildables.GetById(SelectedBuildable.BuildableId);
			if (buildable == null)
			{
				return;
			}

			if (buildable.PlacementStrategy != null && !buildable.PlacementStrategy.Destroy(buildable, this))
			{
				return;
			}

			SelectedBuildable.Selected = false;
			SelectedBuildable = null;
			SelectedBuildable.Destroy();
			Inventory.Add(SelectedBuildable.BuildableId, 1);
		}

		public void Rotate()
		{
			if (SelectedBuildable == null)
			{
				return;
			}

			Buildable buildable = Buildables.GetById(SelectedBuildable.BuildableId);
			if (buildable == null)
			{
				return;
			}

			float nextRotation = buildable.PlacementStrategy?.Rotate(buildable, SelectedBuildable.Rotation.Y, this) ?? SelectedBuildable.Rotation.Y;
			SelectedBuildable.Rotate(nextRotation);
		}

		public override void _Ready()
		{
			base._Ready();

			BuildableChecker.ProcessMode = ProcessModeEnum.Disabled;
			Buildables = GetNode<BuildableDb>("/root/Buildables");
			Navigator.Navigated += HandleSelectHoveredBuildable;
		}
	}
}
