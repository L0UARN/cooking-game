using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class PlayerStateComponent : Node
	{
		[Export]
		private StringName StateName = null;
		[Export]
		private bool IsDefaultState = false;
		[Export]
		private Node3D Container = null;
		[Export]
		private CanvasLayer UiContainer = null;
		[Export]
		private Camera3D Camera = null;

		public override void _Ready()
		{
			PlayerStateManager playerStates = GetNode<PlayerStateManager>("/root/PlayerStates");
			playerStates.RegisterState(StateName, this);

			if (IsDefaultState)
			{
				playerStates.Switch(StateName);
			}
			else
			{
				Deactivate();
			}
		}

		public void Activate()
		{
			Camera.MakeCurrent();
			Container.ProcessMode = ProcessModeEnum.Inherit;
			Container?.Show();
			UiContainer?.Show();
		}

		public void Deactivate()
		{
			Container.ProcessMode = ProcessModeEnum.Disabled;
			Container?.Hide();
			UiContainer?.Hide();
		}
	}
}
