using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuilderCursorComponent : Node
	{
		[Export]
		private BuilderInventoryComponent Inventory = null;
		[Export]
		private GridNavigatorComponent Navigator = null;
		[Export]
		private RayCast3D ExistingChecker = null;

		private BuildableDb Buildables = null;
		private BuildableWrapper SelectedBuildable = null;

		public void Build()
		{
			ExistingChecker.ProcessMode = ProcessModeEnum.Inherit;
			ExistingChecker.ForceRaycastUpdate();
			ExistingChecker.ProcessMode = ProcessModeEnum.Disabled;

			if (ExistingChecker.IsColliding())
			{
				return;
			}

			if (Inventory.SelectedSlot.Quantity <= 0)
			{
				return;
			}

			Buildable buildable = Buildables.GetById(Inventory.SelectedBuildableId);
			if (buildable == null)
			{
				return;
			}

			GridTile currentTile = Navigator.GetTile();
			if (currentTile == null)
			{
				return;
			}

			BuildableWrapper buildableInstance = buildable.Scene.Instantiate<BuildableWrapper>();
			buildableInstance.SuccessfullyPlaced += () => Inventory.Remove(Inventory.SelectedBuildableId, 1);
			buildableInstance.GlobalTransform = currentTile.GlobalTransform;
			GetTree().Root.AddChild(buildableInstance);
		}

		public void Destroy()
		{
			if (SelectedBuildable == null)
			{
				return;
			}

			SelectedBuildable.SuccessfullyDestroyed += () => Inventory.Add(SelectedBuildable.BuildableId, 1);
			SelectedBuildable.Destroy();
			SelectedBuildable = null;
		}

		public void Rotate()
		{
			if (SelectedBuildable == null)
			{
				return;
			}

			SelectedBuildable.Rotate();
		}

		private void HandleSelect()
		{
			ExistingChecker.ProcessMode = ProcessModeEnum.Inherit;
			ExistingChecker.ForceRaycastUpdate();
			ExistingChecker.ProcessMode = ProcessModeEnum.Disabled;

			if (ExistingChecker.GetCollider() is BuildableWrapper buildable)
			{
				if (SelectedBuildable == buildable)
				{
					return;
				}

				if (SelectedBuildable != null)
				{
					SelectedBuildable.Selected = false;
				}

				SelectedBuildable = buildable;
				SelectedBuildable.Selected = true;
			}
			else if (SelectedBuildable != null)
			{
				SelectedBuildable.Selected = false;
				SelectedBuildable = null;
			}
		}

		public override void _Ready()
		{
			base._Ready();

			ExistingChecker.ProcessMode = ProcessModeEnum.Disabled;
			Buildables = GetNode<BuildableDb>("/root/Buildables");
			Navigator.Navigated += HandleSelect;
		}
	}
}
