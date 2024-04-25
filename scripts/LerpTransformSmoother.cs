using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class LerpTransformSmoother : Node3D
	{
		[Export]
		private Timer SmoothingDuration = null;
		[Export]
		private bool IgnoreRotation = false;

		private Node3D Parent = null;
		private Transform3D LastTransform = Transform3D.Identity;
		private Transform3D NextTransform = Transform3D.Identity;

		public override void _Ready()
		{
			base._Ready();

			TopLevel = true;
			Parent = GetParent<Node3D>();
			Reset();
		}

		public void Reset()
		{
			LastTransform = Parent.GlobalTransform;
			NextTransform = LastTransform;
		}

		private void HandleChangeChecks()
		{
			if (Parent.GlobalTransform.IsEqualApprox(NextTransform))
			{
				return;
			}

			LastTransform = GlobalTransform;
			NextTransform = Parent.GlobalTransform;
			SmoothingDuration.Start();
		}

		private void HandleSmoothing()
		{
			if (SmoothingDuration.IsStopped())
			{
				GlobalTransform = NextTransform;
				return;
			}

			double progress = 1.0 - Mathf.Clamp(SmoothingDuration.TimeLeft / SmoothingDuration.WaitTime, 0.0, 1.0);
			double bezierProgress = Mathf.BezierInterpolate(0, 1, 1, 1, progress);
			Transform3D smoothedTransform = LastTransform.InterpolateWith(NextTransform, (float)bezierProgress);

			if (IgnoreRotation)
			{
				GlobalPosition = smoothedTransform.Origin;
			}
			else
			{
				GlobalTransform = smoothedTransform;
			}
		}

		public override void _Process(double delta)
		{
			base._Process(delta);

			HandleChangeChecks();
			HandleSmoothing();
		}
	}
}
