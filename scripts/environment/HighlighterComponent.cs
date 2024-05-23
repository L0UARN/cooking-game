using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class HighlighterComponent : Node
	{
		[Export]
		private Array<MeshInstance3D> Highlightables = new();
		[Export]
		private Material HighlightMaterial = null;

		private bool _Enabled = false;
		public bool Enabled
		{
			get => _Enabled;
			set
			{
				foreach (MeshInstance3D highlightable in Highlightables)
				{
					if (value)
					{
						highlightable.MaterialOverlay = HighlightMaterial;
					}
					else
					{
						highlightable.MaterialOverlay = null;
					}
				}

				_Enabled = value;
			}
		}
	}
}
