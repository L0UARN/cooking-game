using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildableEdgePlacementComponent : Node
	{
		[Export]
		private BuildableWrapper Buildable = null;
		[Export]
		private RayCast3D TileChecker = null;

		private Array<float> PossibleRotations = new();
		private int CurrentRotationIndex = 0;

		private void InitPossibleRotations()
		{
			TileChecker.ProcessMode = ProcessModeEnum.Inherit;

			float[] rotationsToCheck = { 0, Mathf.Pi / 2, Mathf.Pi, -Mathf.Pi / 2 };
			foreach (float rotation in rotationsToCheck)
			{
				TileChecker.Basis = Basis.Identity.Rotated(Vector3.Up, rotation);
				TileChecker.ForceRaycastUpdate();

				if (!TileChecker.IsColliding())
				{
					PossibleRotations.Add(rotation);
				}
			}

			TileChecker.ProcessMode = ProcessModeEnum.Disabled;
		}

		private void HandlePlaced()
		{
			InitPossibleRotations();
			if (PossibleRotations.Count == 0)
			{
				Buildable.QueueFree();
				return;
			}

			Buildable.Basis = Basis.Identity.Rotated(Vector3.Up, PossibleRotations[CurrentRotationIndex]);
			Buildable.ConfirmPlace();
		}

		private void HandleRotated()
		{
			CurrentRotationIndex = (CurrentRotationIndex + 1) % PossibleRotations.Count;
			Buildable.ConfirmRotate(PossibleRotations[CurrentRotationIndex]);
		}

		private void HandleDestroyed()
		{
			Buildable.ConfirmDestroy();
		}

		public override void _Ready()
		{
			base._Ready();

			TileChecker.ProcessMode = ProcessModeEnum.Disabled;
			Buildable.Placed += HandlePlaced;
			Buildable.Rotated += HandleRotated;
			Buildable.Destroyed += HandleDestroyed;
		}
	}
}
