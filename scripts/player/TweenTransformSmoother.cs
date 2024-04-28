using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class TweenTransformSmoother : Node3D
	{
		[Export]
		private float SmoothingDuration = 0.5f;
		[Export]
		private Tween.EaseType EaseType = Tween.EaseType.InOut;
		[Export]
		private Tween.TransitionType TransitionType = Tween.TransitionType.Linear;
		[Export]
		private bool IgnoreRotation = false;

		private Node3D Parent = null;
		private Transform3D NextTransform = Transform3D.Identity;
		private Tween Tween = null;

		public override void _Ready()
		{
			base._Ready();

			TopLevel = true;
			Parent = GetParent<Node3D>();
			Reset();
		}

		public void Reset()
		{
			NextTransform = Parent.GlobalTransform;

			if (Tween?.IsRunning() == true)
			{
				Tween.Pause();
				Tween.CustomStep(1.0f);
				Tween.Kill();
			}
		}

		public override void _Process(double delta)
		{
			base._Process(delta);

			if (Parent.GlobalTransform.IsEqualApprox(NextTransform))
			{
				return;
			}

			NextTransform = Parent.GlobalTransform;
			Tween = GetTree().CreateTween().SetTrans(TransitionType).SetEase(EaseType);
			Tween.TweenProperty(this, "global_transform", NextTransform, SmoothingDuration);
		}
	}
}
