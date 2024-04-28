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
		[Export]
		private PackedScene BuildableWrapper = null;

		public void Build()
		{
			if (ExistingChecker.IsColliding())
			{
				return;
			}

			if (Inventory.SelectedSlot.Quantity <= 0)
			{
				return;
			}

			GridTile currentTile = Navigator.GetTile();
			if (currentTile == null)
			{
				return;
			}

			BuildableWrapper buildableWrapperInstance = BuildableWrapper.Instantiate<BuildableWrapper>();
			buildableWrapperInstance.BuildableId = Inventory.SelectedBuildableId;
			buildableWrapperInstance.GlobalTransform = currentTile.Placement.GlobalTransform;
			GetTree().Root.AddChild(buildableWrapperInstance);

			Inventory.Remove(Inventory.SelectedBuildableId, 1);
		}

		public void Destroy()
		{
			if (ExistingChecker.GetCollider() is BuildableWrapper buildable)
			{
				buildable.Destroy();
				Inventory.Add(buildable.BuildableId, 1);
			}
		}

		public void Rotate()
		{
			if (ExistingChecker.GetCollider() is BuildableWrapper buildable)
			{
				buildable.Rotate();
			}
		}
	}
}
