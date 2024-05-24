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
		[Export]
		private bool Override = false;

		private bool _Enabled = false;
		public bool Enabled
		{
			get => _Enabled;
			set
			{
				foreach (MeshInstance3D highlightable in Highlightables)
				{
					if (Override)
					{
						highlightable.MaterialOverride = value ? HighlightMaterial : null;
					}
					else
					{
						highlightable.MaterialOverlay = value ? HighlightMaterial : null;
					}
				}

				_Enabled = value;
			}
		}
	}
}
