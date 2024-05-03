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

		public override void _Ready()
		{
			base._Ready();
			Buildables = GetNode<BuildableDb>("/root/Buildables");
		}

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
			if (ExistingChecker.GetCollider() is BuildableWrapper buildable)
			{
				buildable.SuccessfullyDestroyed += () => Inventory.Add(buildable.BuildableId, 1);
				buildable.Destroy();
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
