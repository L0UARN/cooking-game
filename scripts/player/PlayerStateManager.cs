using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class PlayerStateManager : Node
	{
		private Dictionary<StringName, PlayerStateComponent> States = new();

		public void RegisterState(StringName name, PlayerStateComponent state)
		{
			States[name] = state;
		}

		public void Switch(StringName name)
		{
			foreach (StringName stateName in States.Keys)
			{
				if (stateName == name)
				{
					States[stateName].Activate();
				}
				else
				{
					States[stateName].Deactivate();
				}
			}
		}
	}
}
