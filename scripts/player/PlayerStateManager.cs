using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class PlayerStateManager : Node
	{
		[Export]
		private CanvasItem TransitionScreen = null;

		private Dictionary<StringName, PlayerStateComponent> States = new();

		public void RegisterState(StringName name, PlayerStateComponent state)
		{
			States[name] = state;
		}

		public void SwitchProcess(StringName name)
		{
			foreach (StringName stateName in States.Keys)
			{
				if (stateName == name)
				{
					States[stateName].ActivateProcess();
				}
				else
				{
					States[stateName].DeactivateProcess();
				}
			}
		}

		public void SwitchVisuals(StringName name)
		{
			foreach (StringName stateName in States.Keys)
			{
				if (stateName == name)
				{
					States[stateName].ActivateVisuals();
				}
				else
				{
					States[stateName].DeactivateVisuals();
				}
			}
		}

		public void Transition(StringName name)
		{
			Tween tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Expo);

			tween.TweenCallback(Callable.From(() =>
			{
				SwitchProcess(name);
			}));

			tween.TweenMethod(Callable.From((float radius) =>
			{
				ShaderMaterial shader = TransitionScreen.Material as ShaderMaterial;
				shader.SetShaderParameter("radius", radius);
			}), 1.0f, 0.0f, 0.5f);

			tween.TweenCallback(Callable.From(() =>
			{
				SwitchVisuals(name);
			}));

			tween.TweenMethod(Callable.From((float radius) =>
			{
				ShaderMaterial shader = TransitionScreen.Material as ShaderMaterial;
				shader.SetShaderParameter("radius", radius);
			}), 0.0f, 1.0f, 0.5f);
		}
	}
}
