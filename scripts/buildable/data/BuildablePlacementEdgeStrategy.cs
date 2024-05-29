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

		public override float Place(Buildable buildable, BuilderCursorComponent cursor)
		{
			float[] wallAngles = GetWallAngles(cursor);
			return wallAngles.Length > 0 ? wallAngles[0] : float.NaN;
		}

		public override bool Destroy(Buildable buildable, BuilderCursorComponent cursor)
		{
			return true;
		}

		public override float Rotate(Buildable buildable, float currentRotation, BuilderCursorComponent cursor)
		{
			float[] wallAngles = GetWallAngles(cursor);
			float closestToCurrentRotation = wallAngles.MinBy(angle => Mathf.Abs(Mathf.AngleDifference(angle, currentRotation)));
			int currentRotationIndex = wallAngles.ToList().FindIndex(direction => Mathf.IsEqualApprox(direction, closestToCurrentRotation));
			float nextRotation = wallAngles[(currentRotationIndex + 1) % wallAngles.Length];
			return nextRotation;
		}
	}
}
