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
		public bool Rotatable { get; set; } = true;
		[Export]
		public bool Removable { get; set; } = true;
	}
}
