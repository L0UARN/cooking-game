using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class ChefInteractorComponent : Node
	{
		[Export]
		private ChefViewComponent View = null;

		private Interactable _Selection = null;
		public Interactable Selection
		{
			get => _Selection;
			set
			{
				if (value != null)
				{
					GD.Print("Selected: " + value.GetPath());
				}

				_Selection = value;
			}
		}

		public bool SelectUp()
		{
			if (Selection?.Up != null)
			{
				Selection = Selection.Up;
				return true;
			}

			return false;
		}

		public bool SelectDown()
		{
			if (Selection?.Down != null)
			{
				Selection = Selection.Down;
				return true;
			}

			return false;
		}

		public bool SelectLeft()
		{
			if (Selection?.Left != null)
			{
				Selection = Selection.Left;
				return true;
			}

			return false;
		}

		public bool SelectRight()
		{
			if (Selection?.Right != null)
			{
				Selection = Selection.Right;
				return true;
			}

			return false;
		}

		private void HandleView(PointOfView pointOfView)
		{
			Selection = pointOfView?.DefaultInteractable;
		}

		public override void _Ready()
		{
			base._Ready();
			View.ViewChanged += HandleView;
		}
	}
}
