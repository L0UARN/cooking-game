using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildableWrapper : Area3D
	{
		[Export]
		public StringName BuildableId { get; set; } = "buildable";

		private BuildableDb Buildables = null;
		private Buildable Buildable = null;

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

			Buildables = GetNode<BuildableDb>("/root/Buildables");
			Buildable = Buildables.GetById(BuildableId);
			if (Buildable == null)
			{
				return;
			}

			Node buildableInstance = Buildable.Scene.Instantiate();
			AddChild(buildableInstance);

			Scale = new Vector3(0.01f, 0.01f, 0.01f);
			ScaleTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Expo);
			ScaleTween.TweenProperty(this, "scale", Vector3.One, .5f);
		}

		public void Rotate()
		{
			if (!Buildable.Rotatable)
			{
				return;
			}

			if (RotateTween?.IsRunning() == true)
			{
				RotateTween.Pause();
				RotateTween.CustomStep(1.0f);
				RotateTween.Kill();
			}

			RotateTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out);
			RotateTween.TweenProperty(this, "rotation:y", Rotation.Y - Mathf.Pi / 2, .5f);
		}

		public void Destroy()
		{
			if (!Buildable.Removable)
			{
				return;
			}

			CollisionLayer = 0;
			CollisionMask = 0;

			if (ScaleTween?.IsRunning() == true)
			{
				ScaleTween.Pause();
				ScaleTween.CustomStep(1.0f);
				ScaleTween.Kill();
			}

			ScaleTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Expo);
			ScaleTween.TweenProperty(this, "scale", new Vector3(0.01f, 0.01f, 0.01f), .5f);
			ScaleTween.TweenCallback(Callable.From(QueueFree));
		}
	}
}
