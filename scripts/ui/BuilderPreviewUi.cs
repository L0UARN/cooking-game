using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuilderPreviewUi : CanvasLayer
	{
		[Export]
		private Node3D Camera = null;
		[Export]
		private Node3D Focus = null;

		private Tween PositionTween = null;
		private Vector3 LastFocusPosition = Vector3.Zero;

		public override void _Ready()
		{
			base._Ready();

			Camera.TopLevel = true;
			LastFocusPosition = Focus.GlobalPosition;
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			if (Focus.GlobalPosition.IsEqualApprox(LastFocusPosition))
			{
				return;
			}

			if (PositionTween?.IsRunning() == true)
			{
				PositionTween.Kill();
			}

			PositionTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic);
			PositionTween.TweenProperty(Camera, "global_position", Focus.GlobalPosition, .5f);

			LastFocusPosition = Focus.GlobalPosition;
		}
	}
}
