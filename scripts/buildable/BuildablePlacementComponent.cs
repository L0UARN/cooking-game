using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildablePlacementComponent : Node
	{
		[Export]
		private BuildableWrapper Buildable = null;

		private float[] PossibleRotations = new float[] { 0.0f, Mathf.Pi * 0.5f, Mathf.Pi, Mathf.Pi * 1.5f };
		private int CurrentRotationIndex = 0;

		private void HandlePlaced()
		{
			Buildable.ConfirmPlace();
		}

		private void HandleRotated()
		{
			CurrentRotationIndex = (CurrentRotationIndex + 1) % PossibleRotations.Length;
			Buildable.ConfirmRotate(PossibleRotations[CurrentRotationIndex]);
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
