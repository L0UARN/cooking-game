using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuildablePlacementEdgeStrategy : BuildablePlacementStrategy
	{
		public override bool Place(Buildable buildable, BuilderCursorComponent cursor)
		{
			Vector3[] wallDirections = { Vector3.Forward, Vector3.Right, Vector3.Back, Vector3.Left };
			bool isNearWall = wallDirections.Any(direction => cursor.CheckForWall(direction));
			return isNearWall;
		}

		public override bool Destroy(Buildable buildable, BuilderCursorComponent cursor)
		{
			return true;
		}

		public override float Rotate(Buildable buildable, float currentRotation, BuilderCursorComponent cursor)
		{
			Vector3[] possibleWallDirections = { Vector3.Forward, Vector3.Right, Vector3.Back, Vector3.Left };
			List<float> wallAngles = possibleWallDirections.Where(direction => cursor.CheckForWall(direction)).Select(direction => direction.AngleTo(Vector3.Forward)).ToList();
			int currentRotationIndex = wallAngles.FindIndex(direction => direction == currentRotation);
			float nextRotation = wallAngles[(currentRotationIndex + 1) % wallAngles.Count];

			return nextRotation;
		}
	}
}
