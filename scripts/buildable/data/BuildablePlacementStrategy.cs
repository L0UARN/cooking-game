using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildablePlacementStrategy : Resource
	{
		public virtual float Place(Buildable buildable, BuilderCursorComponent cursor)
		{
			return 0.0f;
		}

		public virtual bool Destroy(Buildable buildable, BuilderCursorComponent cursor)
		{
			return true;
		}

		public virtual float Rotate(Buildable buildable, float currentRotation, BuilderCursorComponent cursor)
		{
			int rotationAmount = Mathf.RoundToInt(currentRotation / (Mathf.Pi * 0.5f)) % 4;
			float newRotation = (rotationAmount + 1) * Mathf.Pi * 0.5f;
			return newRotation;
		}
	}
}
