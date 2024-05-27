using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildablePlacementStrategy : Resource
	{
		public virtual bool Place(Buildable buildable, BuilderCursorComponent cursor)
		{
			return true;
		}

		public virtual bool Destroy(Buildable buildable, BuilderCursorComponent cursor)
		{
			return true;
		}

		public virtual float Rotate(Buildable buildable, float currentRotation, BuilderCursorComponent cursor)
		{
			return currentRotation - Mathf.Pi * 0.5f;
		}
	}
}
