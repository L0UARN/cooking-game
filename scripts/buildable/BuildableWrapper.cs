using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildableWrapper : Area3D
	{
		[Export]
		public StringName BuildableId { get; set; } = "buildable";
		[Export]
		private Node3D Meshes = null;
		[Export]
		private HighlighterComponent Highlighter = null;
		[Export]
		private HighlighterComponent Downlighter = null;

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

		public override void _Ready()
		{
			base._Ready();

			Downlighter?.AddToGroup("BuildableDownlighters");
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

			Meshes.Scale = new Vector3(0.01f, 0.01f, 0.01f);
			ScaleTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Expo);
			ScaleTween.TweenProperty(Meshes, "scale", Vector3.One, .5f);

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
			ScaleTween.TweenProperty(Meshes, "scale", new Vector3(0.01f, 0.01f, 0.01f), .5f);
			ScaleTween.TweenCallback(Callable.From(QueueFree));

			EmitSignal(SignalName.SuccessfullyDestroyed);
		}

		private bool _Selected = false;
		public bool Selected
		{
			get => _Selected;
			set
			{
				if (Highlighter != null)
				{
					Highlighter.Enabled = value;
				}

				Array<Node> downlighters = GetTree().GetNodesInGroup("BuildableDownlighters");
				foreach (Node other in downlighters)
				{
					if (other != Downlighter && other is HighlighterComponent otherDownlighter)
					{
						otherDownlighter.Enabled = value;
					}
				}

				if (value)
				{
					if (Downlighter != null)
					{
						Downlighter.Enabled = false;
					}
				}

				_Selected = value;
			}
		}
	}
}
