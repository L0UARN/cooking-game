using System.Linq;
using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class RotationVariantComponent : Node
	{
		[Export]
		private Node3D Body = null;
		[Export]
		private Node3D XVariant = null;
		[Export]
		private Node3D ZVariant = null;

		private float LastRotation = float.NaN;

		private void HandleVariantChange()
		{
			Vector3 forward = Body.GlobalBasis.Z;
			Array<Vector3> possibleDirections = new() { Vector3.Forward, Vector3.Left, Vector3.Back, Vector3.Right };
			Vector3 closestDirection = possibleDirections.MaxBy(direction => forward.Dot(direction));

			if (closestDirection.IsEqualApprox(Vector3.Forward) || closestDirection.IsEqualApprox(Vector3.Back))
			{
				XVariant.Hide();
				XVariant.ProcessMode = ProcessModeEnum.Disabled;
				ZVariant.Show();
				ZVariant.ProcessMode = ProcessModeEnum.Inherit;

				GD.Print(GetPath(), " is facing Z");
			}
			else
			{
				XVariant.Show();
				XVariant.ProcessMode = ProcessModeEnum.Inherit;
				ZVariant.Hide();
				ZVariant.ProcessMode = ProcessModeEnum.Disabled;

				GD.Print(GetPath(), " is facing X");
			}
		}

		public override void _Ready()
		{
			base._Ready();

			LastRotation = Body.GlobalRotation.Y;
			HandleVariantChange();
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			if (Body.GlobalRotation.Y != LastRotation)
			{
				HandleVariantChange();
				LastRotation = Body.GlobalRotation.Y;
			}
		}
	}
}
