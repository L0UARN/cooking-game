using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class PointOfView : Area3D
	{
		[Export]
		public Node3D Placement { get; set; } = null;
	}
}
