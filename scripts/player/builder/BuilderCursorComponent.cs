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
			GD.Print(Time.GetTicksMsec());

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

			if (buildable.PlacementStrategy != null && !buildable.PlacementStrategy.Place(buildable, this))
			{
				return;
			}

			Inventory.Remove(Inventory.SelectedBuildableId, 1);
			Node3D buildableInstance = buildable.Scene.Instantiate<Node3D>();
			buildableInstance.GlobalTransform = currentTile.GlobalTransform;
			GetTree().Root.AddChild(buildableInstance);
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

			BuildableComponent toDestroy = SelectedBuildable;
			SelectedBuildable = null;
			Inventory.Add(toDestroy.BuildableId, 1);
			toDestroy.Destroy();
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

			float nextRotation = buildable.PlacementStrategy?.Rotate(buildable, SelectedBuildable.GlobalRotation.Y, this) ?? SelectedBuildable.GlobalRotation.Y;
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
