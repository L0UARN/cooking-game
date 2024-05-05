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
				playerStates.SwitchProcess(StateName);
				playerStates.SwitchVisuals(StateName);
			}
		}

		public void ActivateProcess()
		{
			Container.ProcessMode = ProcessModeEnum.Inherit;
		}

		public void DeactivateProcess()
		{
			Container.ProcessMode = ProcessModeEnum.Disabled;
		}

		public void ActivateVisuals()
		{
			Camera.MakeCurrent();
			Container?.Show();
			UiContainer?.Show();
		}

		public void DeactivateVisuals()
		{
			Container?.Hide();
			UiContainer?.Hide();
		}
	}
}
