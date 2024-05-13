using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class Item : Resource
	{
		[Export]
		public StringName Id { get; set; } = "item";
		[Export]
		public string Name { get; set; } = "Item";
		[Export]
		public PackedScene Scene { get; set; } = null;
	}
}
