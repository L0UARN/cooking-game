using System.Linq;
using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildablePlacementEdgeStrategy : BuildablePlacementStrategy
	{
		private static float[] GetWallAngles(BuilderCursorComponent cursor)
		{
			float[] possibleWallAngles = { 0, Mathf.Pi * 0.5f, Mathf.Pi, Mathf.Pi * 1.5f };
			float[] wallAngles = possibleWallAngles.Where(angle => cursor.GetTileInDirection(angle) == null).ToArray();
			return wallAngles;
		}

		public override bool Place(Buildable buildable, BuilderCursorComponent cursor)
		{
			return GetWallAngles(cursor).Length > 0;
		}

		public override bool Destroy(Buildable buildable, BuilderCursorComponent cursor)
		{
			return true;
		}

		public override float Rotate(Buildable buildable, float currentRotation, BuilderCursorComponent cursor)
		{
			float[] wallAngles = GetWallAngles(cursor);
			int currentRotationIndex = wallAngles.ToList().FindIndex(direction => direction == currentRotation);
			float nextRotation = wallAngles[(currentRotationIndex + 1) % wallAngles.Length];
			return nextRotation;
		}
	}
}
