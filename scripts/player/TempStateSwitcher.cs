using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class TempStateSwitcher : Node
	{
		public override void _Input(InputEvent inputEvent)
		{
			base._Input(inputEvent);

			if (inputEvent.IsActionPressed("Switch"))
			{
				PlayerStateManager playerStates = GetNode<PlayerStateManager>("/root/PlayerStates");
				playerStates.Transition("chef");
				GD.Print("Switching to chef state");
			}
		}
	}
}
