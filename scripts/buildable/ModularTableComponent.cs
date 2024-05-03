using Godot;
using System;

namespace CookingGame
{
	[GlobalClass]
	public partial class ModularTableComponent : Node
	{
		[Export]
		private RayCast3D UpDetector = null;
		[Export]
		private RayCast3D DownDetector = null;
		[Export]
		private RayCast3D LeftDetector = null;
		[Export]
		private RayCast3D RightDetector = null;

		[Export]
		private MeshInstance3D XTop = null;
		[Export]
		private MeshInstance3D TTop = null;
		[Export]
		private MeshInstance3D LTop = null;
		[Export]
		private MeshInstance3D ITop = null;
		[Export]
		private MeshInstance3D DTop = null;
		[Export]
		private MeshInstance3D OTop = null;

		[Export]
		private Godot.Collections.Array<MeshInstance3D> Bases = new();

		private int LastDetectionCount = 0;
		private Vector3 LastUpVector = Vector3.Zero;

		private int GetDetectionCount()
		{
			int detectionCount = 0;

			if (UpDetector.IsColliding())
			{
				detectionCount++;
			}
			if (DownDetector.IsColliding())
			{
				detectionCount++;
			}
			if (LeftDetector.IsColliding())
			{
				detectionCount++;
			}
			if (RightDetector.IsColliding())
			{
				detectionCount++;
			}

			return detectionCount;
		}

		private void EnableMesh(MeshInstance3D mesh, bool enable)
		{
			if (enable)
			{
				mesh.Show();
				mesh.ProcessMode = ProcessModeEnum.Inherit;
			}
			else
			{
				mesh.Hide();
				mesh.ProcessMode = ProcessModeEnum.Disabled;
			}
		}

		private void EnableXTop()
		{
			EnableMesh(XTop, true);
			EnableMesh(TTop, false);
			EnableMesh(LTop, false);
			EnableMesh(ITop, false);
			EnableMesh(DTop, false);
			EnableMesh(OTop, false);
		}

		private void EnableTTop()
		{
			TTop.Basis = Basis.Identity;
			if (!RightDetector.IsColliding())
			{
				TTop.RotateY(0);
			}
			else if (!DownDetector.IsColliding())
			{
				TTop.RotateY(Mathf.Pi * -0.5f);
			}
			else if (!LeftDetector.IsColliding())
			{
				TTop.RotateY(Mathf.Pi * -1.0f);
			}
			else if (!UpDetector.IsColliding())
			{
				TTop.RotateY(Mathf.Pi * -1.5f);
			}

			EnableMesh(XTop, false);
			EnableMesh(TTop, true);
			EnableMesh(LTop, false);
			EnableMesh(ITop, false);
			EnableMesh(DTop, false);
			EnableMesh(OTop, false);
		}

		private void EnableLTop()
		{
			LTop.Basis = Basis.Identity;
			if (DownDetector.IsColliding() && LeftDetector.IsColliding())
			{
				LTop.RotateY(0);
			}
			else if (LeftDetector.IsColliding() && UpDetector.IsColliding())
			{
				LTop.RotateY(Mathf.Pi * -0.5f);
			}
			else if (UpDetector.IsColliding() && RightDetector.IsColliding())
			{
				LTop.RotateY(Mathf.Pi * -1.0f);
			}
			else if (RightDetector.IsColliding() && DownDetector.IsColliding())
			{
				LTop.RotateY(Mathf.Pi * -1.5f);
			}

			EnableMesh(XTop, false);
			EnableMesh(TTop, false);
			EnableMesh(LTop, true);
			EnableMesh(ITop, false);
			EnableMesh(DTop, false);
			EnableMesh(OTop, false);
		}

		private void EnableITop()
		{
			ITop.Basis = Basis.Identity;
			if (UpDetector.IsColliding() && DownDetector.IsColliding())
			{
				ITop.RotateY(0);
			}
			else if (LeftDetector.IsColliding() && RightDetector.IsColliding())
			{
				ITop.RotateY(Mathf.Pi * -0.5f);
			}

			EnableMesh(XTop, false);
			EnableMesh(TTop, false);
			EnableMesh(LTop, false);
			EnableMesh(ITop, true);
			EnableMesh(DTop, false);
			EnableMesh(OTop, false);
		}

		private void EnableDTop()
		{
			DTop.Basis = Basis.Identity;
			if (DownDetector.IsColliding())
			{
				DTop.RotateY(0);
			}
			else if (LeftDetector.IsColliding())
			{
				DTop.RotateY(Mathf.Pi * -0.5f);
			}
			else if (UpDetector.IsColliding())
			{
				DTop.RotateY(Mathf.Pi * -1.0f);
			}
			else if (RightDetector.IsColliding())
			{
				DTop.RotateY(Mathf.Pi * -1.5f);
			}

			EnableMesh(XTop, false);
			EnableMesh(TTop, false);
			EnableMesh(LTop, false);
			EnableMesh(ITop, false);
			EnableMesh(DTop, true);
			EnableMesh(OTop, false);
		}

		private void EnableOTop()
		{
			EnableMesh(XTop, false);
			EnableMesh(TTop, false);
			EnableMesh(LTop, false);
			EnableMesh(ITop, false);
			EnableMesh(DTop, false);
			EnableMesh(OTop, true);
		}

		private void EnableLOrITop()
		{
			if ((LeftDetector.IsColliding() && RightDetector.IsColliding()) || (UpDetector.IsColliding() && DownDetector.IsColliding()))
			{
				EnableITop();
			}
			else
			{
				EnableLTop();
			}
		}

		private void EnableRandomBase()
		{
			Random random = new();
			int chosenBase = random.Next(Bases.Count);

			for (int i = 0; i < Bases.Count; i++)
			{
				if (i == chosenBase)
				{
					EnableMesh(Bases[i], true);
				}
				else
				{
					EnableMesh(Bases[i], false);
				}
			}
		}

		public override void _Ready()
		{
			base._Ready();

			// Show only the OTop
			EnableOTop();

			// Select a random base to show
			EnableRandomBase();
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			int detectionCount = GetDetectionCount();
			if (LastDetectionCount == detectionCount && LastUpVector.IsEqualApprox(UpDetector.GlobalBasis.Z))
			{
				return;
			}

			if ( detectionCount == 0)
			{
				EnableOTop();
			}
			else if (detectionCount == 1)
			{
				EnableDTop();
			}
			else if (detectionCount == 2)
			{
				EnableLOrITop();
			}
			else if (detectionCount == 3)
			{
				EnableTTop();
			}
			else if (detectionCount == 4)
			{
				EnableXTop();
			}

			LastDetectionCount = detectionCount;
			LastUpVector = UpDetector.GlobalBasis.Z;
		}
	}
}
