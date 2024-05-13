using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class ItemDisplay : Node3D
	{
		private Node ItemSceneInstance = null;

		private Item _Item = null;
		public Item Item
		{
			get => _Item;
			set
			{
				_Item = value;
				UpdateDisplay();
			}
		}

		private void UpdateDisplay()
		{
			ItemSceneInstance?.QueueFree();

			if (Item == null)
			{
				ItemSceneInstance = null;
				return;
			}

			ItemSceneInstance = Item.Scene.Instantiate();
			AddChild(ItemSceneInstance);
		}
	}
}
