using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildableWrapper : Area3D
	{
		[Export]
		public StringName BuildableId { get; set; } = "buildable";

		private Tween ScaleTween = null;
		private Tween RotateTween = null;

		public override void _Ready()
		{
			base._Ready();

			if (ScaleTween?.IsRunning() == true)
			{
				ScaleTween.Pause();
				ScaleTween.CustomStep(1.0f);
				ScaleTween.Kill();
			}

			Scale = new Vector3(0.01f, 0.01f, 0.01f);
			ScaleTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Elastic);
			ScaleTween.TweenProperty(this, "scale", Vector3.One, .5f);
		}

		public void Destroy()
		{
			CollisionLayer = 0;
			CollisionMask = 0;

			if (ScaleTween?.IsRunning() == true)
			{
				ScaleTween.Pause();
				ScaleTween.CustomStep(1.0f);
				ScaleTween.Kill();
			}

			ScaleTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Elastic);
			ScaleTween.TweenProperty(this, "scale", new Vector3(0.01f, 0.01f, 0.01f), .5f);
			ScaleTween.TweenCallback(Callable.From(QueueFree));
		}

		public void Rotate()
		{
			if (RotateTween?.IsRunning() == true)
			{
				RotateTween.Pause();
				RotateTween.CustomStep(1.0f);
				RotateTween.Kill();
			}

			RotateTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out);
			RotateTween.TweenProperty(this, "rotation:y", Rotation.Y - Mathf.Pi / 2, .5f);
		}
	}
}
