using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuilderPreviewUi : MarginContainer
	{
		[Export]
		private Node3D Camera = null;
		[Export]
		private Node3D Focus = null;

		public override void _Ready()
		{
			base._Ready();
			Camera.TopLevel = true;
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
			Camera.GlobalPosition = Focus.GlobalPosition;
		}
	}
}
