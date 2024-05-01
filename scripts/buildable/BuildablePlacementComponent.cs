using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildablePlacementComponent : Node
	{
		[Export]
		private BuildableWrapper Buildable = null;

		private void HandlePlaced()
		{
			Buildable.ConfirmPlace();
		}

		private void HandleRotated()
		{
			Buildable.ConfirmRotate(Buildable.Rotation.Y + Mathf.Pi / 2.0f);
		}

		private void HandleDestroyed()
		{
			Buildable.ConfirmDestroy();
		}

		public override void _Ready()
		{
			base._Ready();

			Buildable.Placed += HandlePlaced;
			Buildable.Rotated += HandleRotated;
			Buildable.Destroyed += HandleDestroyed;
		}
	}
}
