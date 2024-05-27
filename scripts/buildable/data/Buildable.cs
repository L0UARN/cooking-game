using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class Buildable : Resource
	{
		[Export]
		public StringName Id { get; set; } = "buildable";
		[Export]
		public string Name { get; set; } = "Buildable";
		[Export]
		public Texture2D Icon { get; set; } = null;
		[Export]
		public PackedScene Scene { get; set; } = null;

		[Export]
		public BuildablePlacementStrategy PlacementStrategy { get; set; } = null;
		[Export]
		public PackedScene AdjascentWall { get; set; } = null;
		[Export]
		public bool IsSolid { get; set; } = true;
	}
}
