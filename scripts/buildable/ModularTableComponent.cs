using Godot;
using System;

namespace Prototypes
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
		private Node3D XTop = null;
		[Export]
		private Node3D TTop = null;
		[Export]
		private Node3D LTop = null;
		[Export]
		private Node3D ITop = null;
		[Export]
		private Node3D DTop = null;
		[Export]
		private Node3D OTop = null;

		[Export]
		private Godot.Collections.Array<Node3D> Bases = new();

		private int LastDetectionCount = 0;
		private Vector3 LastUpVector = Vector3.Zero;

		public override void _Ready()
		{
			base._Ready();

			// Show only the OTop
			EnableOTop();

			// Select a random base to show
			Random random = new();
			int chosenBase = random.Next(Bases.Count);

			for (int i = 0; i < Bases.Count; i++)
			{
				if (i == chosenBase)
				{
					Bases[i].Show();
					Bases[i].ProcessMode = ProcessModeEnum.Inherit;
				}
				else
				{
					Bases[i].Hide();
					Bases[i].ProcessMode = ProcessModeEnum.Disabled;
				}
			}
		}

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

		private void EnableXTop()
		{
			XTop.Show();
			XTop.ProcessMode = ProcessModeEnum.Inherit;
			TTop.Hide();
			TTop.ProcessMode = ProcessModeEnum.Disabled;
			LTop.Hide();
			LTop.ProcessMode = ProcessModeEnum.Disabled;
			ITop.Hide();
			ITop.ProcessMode = ProcessModeEnum.Disabled;
			DTop.Hide();
			DTop.ProcessMode = ProcessModeEnum.Disabled;
			OTop.Hide();
			OTop.ProcessMode = ProcessModeEnum.Disabled;
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

			XTop.Hide();
			XTop.ProcessMode = ProcessModeEnum.Disabled;
			TTop.Show();
			TTop.ProcessMode = ProcessModeEnum.Inherit;
			LTop.Hide();
			LTop.ProcessMode = ProcessModeEnum.Disabled;
			ITop.Hide();
			ITop.ProcessMode = ProcessModeEnum.Disabled;
			DTop.Hide();
			DTop.ProcessMode = ProcessModeEnum.Disabled;
			OTop.Hide();
			OTop.ProcessMode = ProcessModeEnum.Disabled;
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

			XTop.Hide();
			XTop.ProcessMode = ProcessModeEnum.Disabled;
			TTop.Hide();
			TTop.ProcessMode = ProcessModeEnum.Disabled;
			LTop.Show();
			LTop.ProcessMode = ProcessModeEnum.Inherit;
			ITop.Hide();
			ITop.ProcessMode = ProcessModeEnum.Disabled;
			DTop.Hide();
			DTop.ProcessMode = ProcessModeEnum.Disabled;
			OTop.Hide();
			OTop.ProcessMode = ProcessModeEnum.Disabled;
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

			XTop.Hide();
			XTop.ProcessMode = ProcessModeEnum.Disabled;
			TTop.Hide();
			TTop.ProcessMode = ProcessModeEnum.Disabled;
			LTop.Hide();
			LTop.ProcessMode = ProcessModeEnum.Disabled;
			ITop.Show();
			ITop.ProcessMode = ProcessModeEnum.Inherit;
			DTop.Hide();
			DTop.ProcessMode = ProcessModeEnum.Disabled;
			OTop.Hide();
			OTop.ProcessMode = ProcessModeEnum.Disabled;
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

			XTop.Hide();
			XTop.ProcessMode = ProcessModeEnum.Disabled;
			TTop.Hide();
			TTop.ProcessMode = ProcessModeEnum.Disabled;
			LTop.Hide();
			LTop.ProcessMode = ProcessModeEnum.Disabled;
			ITop.Hide();
			ITop.ProcessMode = ProcessModeEnum.Disabled;
			DTop.Show();
			DTop.ProcessMode = ProcessModeEnum.Inherit;
			OTop.Hide();
			OTop.ProcessMode = ProcessModeEnum.Disabled;
		}

		private void EnableOTop()
		{
			XTop.Hide();
			XTop.ProcessMode = ProcessModeEnum.Disabled;
			TTop.Hide();
			TTop.ProcessMode = ProcessModeEnum.Disabled;
			LTop.Hide();
			LTop.ProcessMode = ProcessModeEnum.Disabled;
			ITop.Hide();
			ITop.ProcessMode = ProcessModeEnum.Disabled;
			DTop.Hide();
			DTop.ProcessMode = ProcessModeEnum.Disabled;
			OTop.Show();
			OTop.ProcessMode = ProcessModeEnum.Inherit;
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
