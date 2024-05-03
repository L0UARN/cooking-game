using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildableWrapper : Area3D
	{
		[Export]
		public StringName BuildableId { get; set; } = "buildable";

		[Signal]
		public delegate void PlacedEventHandler();
		[Signal]
		public delegate void RotatedEventHandler();
		[Signal]
		public delegate void DestroyedEventHandler();

		[Signal]
		public delegate void SuccessfullyPlacedEventHandler();
		[Signal]
		public delegate void SuccessfullyRotatedEventHandler();
		[Signal]
		public delegate void SuccessfullyDestroyedEventHandler();

		private Tween ScaleTween = null;
		private Tween RotateTween = null;
		private Tween PositionTween = null;

		public override void _Ready()
		{
			base._Ready();
			EmitSignal(SignalName.Placed);
		}

		public void ConfirmPlace()
		{
			if (ScaleTween?.IsRunning() == true)
			{
				ScaleTween.Pause();
				ScaleTween.CustomStep(1.0f);
				ScaleTween.Kill();
			}

			Scale = new Vector3(0.01f, 0.01f, 0.01f);
			ScaleTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Expo);
			ScaleTween.TweenProperty(this, "scale", Vector3.One, .5f);

			EmitSignal(SignalName.SuccessfullyPlaced);
		}

		public void Rotate()
		{
			EmitSignal(SignalName.Rotated);
		}

		public void ConfirmRotate(float angle)
		{
			if (RotateTween?.IsRunning() == true)
			{
				RotateTween.Pause();
				RotateTween.CustomStep(1.0f);
				RotateTween.Kill();
			}

			RotateTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out);
			RotateTween.TweenProperty(this, "rotation:y", angle, .5f);

			EmitSignal(SignalName.SuccessfullyRotated);
		}

		public void Destroy()
		{
			EmitSignal(SignalName.Destroyed);
		}

		public void ConfirmDestroy()
		{
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

			EmitSignal(SignalName.SuccessfullyDestroyed);
		}

		private bool _Selected = false;
		public bool Selected
		{
			get => _Selected;
			set
			{
				if (PositionTween?.IsRunning() == true)
				{
					PositionTween.Pause();
					PositionTween.CustomStep(1.0f);
					PositionTween.Kill();
				}

				if (value)
				{
					PositionTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Expo);
					PositionTween.TweenProperty(this, "position:y", Position.Y + 0.5f, .5f);
				}
				else
				{
					PositionTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Expo);
					PositionTween.TweenProperty(this, "position:y", Position.Y - 0.5f, .5f);
				}

				_Selected = value;
			}
		}
	}
}
