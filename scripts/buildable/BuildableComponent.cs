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

		private Tween ScaleTween = null;
		private Tween RotateTween = null;

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

		public void Destroy()
		{
			// TODO: animate the pop-out
			Body.QueueFree();
		}

		public void Rotate(float angle)
		{
			// TODO: animate the rotation
			Body.Rotate(Vector3.Up, angle);
		}

		public override void _Ready()
		{
			base._Ready();
			// TODO: animate the pop-in
			Downlighter?.AddToGroup("BuildableDownlighters");
		}
	}
}
