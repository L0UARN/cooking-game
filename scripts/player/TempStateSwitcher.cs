using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class TempStateSwitcher : Node
	{
		public override void _Input(InputEvent inputEvent)
		{
			base._Input(inputEvent);

			if (inputEvent.IsAction("Switch"))
			{
				PlayerStateManager playerStates = GetNode<PlayerStateManager>("/root/PlayerStates");
				playerStates.Switch("chef");
			}
		}
	}
}
