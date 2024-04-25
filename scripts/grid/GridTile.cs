using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class GridTile : Area3D
	{
		[Export]
		public Node3D Placement = null;
	}
}
