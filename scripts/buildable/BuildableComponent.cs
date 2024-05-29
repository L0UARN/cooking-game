using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildableComponent : Area3D
	{
		[Export]
		public StringName BuildableId { get; set; } = "buildable";

		[Export]
		private Node3D Body = null;
		[Export]
		private HighlighterComponent Highlighter = null;
		[Export]
		private HighlighterComponent Downlighter = null;

		private bool _Highlited = false;
		public bool Highlighted
		{
			get => _Highlited;
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

				_Highlited = value;
			}
		}

		private Tween Tween = null;

		private void ResetTween(bool hard = false)
		{
			if (Tween?.IsRunning() == true)
			{
				if (hard)
				{
					Tween.Pause();
					Tween.CustomStep(999.9f);
				}

				Tween.Kill();
			}
		}

		public void Destroy()
		{
			// Remove collision from this area to avoid the buildable from being detected while it is being destroyed
			CollisionLayer = 0;
			CollisionMask = 0;

			ResetTween();
			Tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.InOut);
			Tween.TweenProperty(Body, "scale", new Vector3(0.001f, 0.001f, 0.001f), .5f);
			Tween.TweenCallback(Callable.From(Body.QueueFree));
		}

		public void Rotate(float angle)
		{
			ResetTween();
			Tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out);
			Tween.TweenProperty(Body, "global_rotation:y", angle, .5f);
		}

		public override void _Ready()
		{
			base._Ready();

			Downlighter?.AddToGroup("BuildableDownlighters");
			Body.Scale = new Vector3(0.001f, 0.001f, 0.001f);

			ResetTween();
			Tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out);
			Tween.TweenProperty(Body, "scale", Vector3.One, .5f);
		}
	}
}
