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
		private Node3D TopContainer = null;
		[Export]
		private MeshInstance3D XTop = null;
		[Export]
		private MeshInstance3D TTop = null;
		[Export]
		private MeshInstance3D LTop = null;
		[Export]
		private MeshInstance3D ITop = null;
		[Export]
		private MeshInstance3D UTop = null;
		[Export]
		private MeshInstance3D OTop = null;

		[Export]
		private Node3D BaseContainer = null;
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

		private void EnableBase(MeshInstance3D mesh, bool enable)
		{
			if (enable && mesh.GetParent() == null)
			{
				BaseContainer.AddChild(mesh);
			}
			else if (!enable && mesh.GetParent() == BaseContainer)
			{
				BaseContainer.RemoveChild(mesh);
			}
		}

		private void EnableTop(MeshInstance3D mesh, bool enable)
		{
			if (enable && mesh.GetParent() == null)
			{
				TopContainer.AddChild(mesh);
			}
			else if (!enable && mesh.GetParent() == TopContainer)
			{
				TopContainer.RemoveChild(mesh);
			}
		}

		private void EnableXTop()
		{
			EnableTop(XTop, true);
			EnableTop(TTop, false);
			EnableTop(LTop, false);
			EnableTop(ITop, false);
			EnableTop(UTop, false);
			EnableTop(OTop, false);
		}

		private void EnableTTop()
		{
			EnableTop(XTop, false);
			EnableTop(TTop, true);
			EnableTop(LTop, false);
			EnableTop(ITop, false);
			EnableTop(UTop, false);
			EnableTop(OTop, false);

			TTop.Basis = Basis.Identity;
			if (!LeftDetector.IsColliding())
			{
				TTop.RotateY(0);
			}
			else if (!UpDetector.IsColliding())
			{
				TTop.RotateY(Mathf.Pi * -0.5f);
			}
			else if (!RightDetector.IsColliding())
			{
				TTop.RotateY(Mathf.Pi * -1.0f);
			}
			else if (!DownDetector.IsColliding())
			{
				TTop.RotateY(Mathf.Pi * -1.5f);
			}
		}

		private void EnableLTop()
		{
			EnableTop(XTop, false);
			EnableTop(TTop, false);
			EnableTop(LTop, true);
			EnableTop(ITop, false);
			EnableTop(UTop, false);
			EnableTop(OTop, false);

			LTop.Basis = Basis.Identity;
			if (UpDetector.IsColliding() && RightDetector.IsColliding())
			{
				LTop.RotateY(0);
			}
			else if (DownDetector.IsColliding() && RightDetector.IsColliding())
			{
				LTop.RotateY(Mathf.Pi * -0.5f);
			}
			else if (DownDetector.IsColliding() && LeftDetector.IsColliding())
			{
				LTop.RotateY(Mathf.Pi * -1.0f);
			}
			else if (UpDetector.IsColliding() && LeftDetector.IsColliding())
			{
				LTop.RotateY(Mathf.Pi * -1.5f);
			}
		}

		private void EnableITop()
		{
			EnableTop(XTop, false);
			EnableTop(TTop, false);
			EnableTop(LTop, false);
			EnableTop(ITop, true);
			EnableTop(UTop, false);
			EnableTop(OTop, false);

			ITop.Basis = Basis.Identity;
			if (UpDetector.IsColliding() && DownDetector.IsColliding())
			{
				ITop.RotateY(0);
			}
			else if (LeftDetector.IsColliding() && RightDetector.IsColliding())
			{
				ITop.RotateY(Mathf.Pi * -0.5f);
			}
		}

		private void EnableUTop()
		{
			EnableTop(XTop, false);
			EnableTop(TTop, false);
			EnableTop(LTop, false);
			EnableTop(ITop, false);
			EnableTop(UTop, true);
			EnableTop(OTop, false);

			UTop.Basis = Basis.Identity;
			if (UpDetector.IsColliding())
			{
				UTop.RotateY(0);
			}
			else if (RightDetector.IsColliding())
			{
				UTop.RotateY(Mathf.Pi * -0.5f);
			}
			else if (DownDetector.IsColliding())
			{
				UTop.RotateY(Mathf.Pi * -1.0f);
			}
			else if (LeftDetector.IsColliding())
			{
				UTop.RotateY(Mathf.Pi * -1.5f);
			}
		}

		private void EnableOTop()
		{
			EnableTop(XTop, false);
			EnableTop(TTop, false);
			EnableTop(LTop, false);
			EnableTop(ITop, false);
			EnableTop(UTop, false);
			EnableTop(OTop, true);
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
					EnableBase(Bases[i], true);
				}
				else
				{
					EnableBase(Bases[i], false);
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
				EnableUTop();
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
