using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuilderInventorySlotUi : PanelContainer
	{
		[Export]
		public Container Container = null;
		[Export]
		public Label NameLabel = null;
		[Export]
		public Label QuantityLabel = null;
		[Export]
		public TextureRect IconTexture = null;

		private BuildableDb Buildables = null;
		private StyleBoxFlat Style = null;
		private Tween SlotTween = null;
		private Tween SelectedTween = null;

		private BuildableInventorySlot _Slot = null;
		public BuildableInventorySlot Slot
		{
			get => _Slot;
			set
			{
				_Slot = value;
				if (IsNodeReady())
				{
					RefreshSlot();
				}
			}
		}

		private void RefreshSlot()
		{
			if (Slot == null)
			{
				NameLabel.Text = "";
				QuantityLabel.Text = "";
				IconTexture.Texture = null;
				Hide();
			}
			else
			{
				Buildable buildable = Buildables.GetById(Slot.BuildableId);

				NameLabel.Text = buildable.Name;
				QuantityLabel.Text = "x" + Slot.Quantity.ToString();
				IconTexture.Texture = buildable.Icon;
				Show();

				PivotOffset = Size / 2;

				if (SlotTween?.IsRunning() == true)
				{
					SlotTween.Pause();
					SlotTween.CustomStep(1.0f);
					SlotTween.Kill();
				}

				SlotTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic);
				SlotTween.TweenProperty(this, "scale", new Vector2(1.1f, 1.1f), .16f);
				SlotTween.TweenProperty(this, "scale", new Vector2(1.0f, 1.0f), .16f);
			}
		}

		private bool _Selected = false;
		public bool Selected
		{
			get => _Selected;
			set
			{
				_Selected = value;
				if (IsNodeReady())
				{
					RefreshSelected();
				}
			}
		}

		private void RefreshSelected()
		{
			if (SelectedTween?.IsRunning() == true)
			{
				SelectedTween.Pause();
				SelectedTween.CustomStep(1.0f);
				SelectedTween.Kill();
			}

			SelectedTween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Sine);

			if (Selected)
			{
				SelectedTween.Parallel().TweenProperty(this, "scale", new Vector2(1.1f, 1.1f), .16f);
				SelectedTween.Parallel().TweenProperty(Style, "bg_color", new Color(1, 1, 1), .16f);
				SelectedTween.Parallel().TweenProperty(Container, "modulate", new Color(0, 0, 0), .16f);
				SelectedTween.TweenProperty(this, "scale", new Vector2(1.0f, 1.0f), .16f);
			}
			else
			{
				SelectedTween.Parallel().TweenProperty(Style, "bg_color", new Color(0, 0, 0), .16f);
				SelectedTween.Parallel().TweenProperty(Container, "modulate", new Color(1, 1, 1), .16f);
			}
		}

		public override void _Ready()
		{
			base._Ready();

			Buildables = GetNode<BuildableDb>("/root/Buildables");
			Style = GetThemeStylebox("panel").Duplicate() as StyleBoxFlat;
			AddThemeStyleboxOverride("panel", Style);

			RefreshSlot();
			RefreshSelected();
		}
	}
}
