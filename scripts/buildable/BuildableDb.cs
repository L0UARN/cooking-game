using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildableDb : Node
	{
		[Export]
		private Array<Buildable> Buildables = new();

		public Buildable GetById(StringName id)
		{
			foreach (var buildable in Buildables)
			{
				if (buildable.Id == id)
				{
					return buildable;
				}
			}

			return null;
		}
	}
}
