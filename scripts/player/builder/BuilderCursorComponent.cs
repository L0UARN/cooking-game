using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuilderCursorComponent : GridNavigatorComponent
	{
		[Export]
		public BuilderInventoryComponent Inventory { get; set; } = null;
		[Export]
		private RayCast3D BuildableChecker = null;

		private BuildableDb Buildables = null;

		private BuildableComponent _SelectedBuildable = null;
		public BuildableComponent SelectedBuildable
		{
			get => _SelectedBuildable;
			set
			{
				if (_SelectedBuildable != null)
				{
					_SelectedBuildable.Highlighted = false;
				}

				if (value != null)
				{
					value.Highlighted = true;
				}

				_SelectedBuildable = value;
			}
		}

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

		private void SelectHoveredBuildable()
		{
			BuildableComponent hoveredBuildable = GetHoveredBuildable();

			if (SelectedBuildable == hoveredBuildable)
			{
				return;
			}

			if (hoveredBuildable == null && SelectedBuildable != null)
			{
				if (SelectedBuildable != null)
				{
					SelectedBuildable = null;
				}

				return;
			}

			SelectedBuildable = hoveredBuildable;
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

			GridTile currentTile = GetHoveredTile();
			if (currentTile == null)
			{
				return;
			}

			float initialRotation = buildable.PlacementStrategy?.Place(buildable, this) ?? float.NaN;
			if (float.IsNaN(initialRotation))
			{
				return;
			}

			Inventory.Remove(Inventory.SelectedBuildableId, 1);
			Node3D buildableInstance = buildable.Scene.Instantiate<Node3D>();
			buildableInstance.GlobalTransform = currentTile.GlobalTransform;
			currentTile.GetParent().AddChild(buildableInstance);
			buildableInstance.GlobalRotate(Vector3.Up, initialRotation);
			SelectHoveredBuildable();
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

			Inventory.Add(SelectedBuildable.BuildableId, 1);
			SelectedBuildable.Destroy();
			SelectedBuildable = null;
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

			float nextRotation = buildable.PlacementStrategy?.Rotate(buildable, SelectedBuildable.GlobalRotation.Y, this) ?? float.NaN;
			if (float.IsNaN(nextRotation))
			{
				return;
			}

			SelectedBuildable.Rotate(nextRotation);
		}

		public override void _Ready()
		{
			base._Ready();

			BuildableChecker.ProcessMode = ProcessModeEnum.Disabled;
			Buildables = GetNode<BuildableDb>("/root/Buildables");
			Navigated += SelectHoveredBuildable;
		}
	}
}
