using System.Linq;
using Godot;

namespace CookingGame
{
	[GlobalClass]
	public partial class ChefInteractorInputComponent : Node
	{
		[Export]
		private ChefInteractorComponent Interactor = null;
		[Export]
		private Timer SelectCooldown = null;
		[Export]
		private Timer InteractCooldown = null;
		[Export]
		private RayCast3D MouseSelectRay = null;

		private bool ShouldDoMouseSelection = false;
		private InputInstance InputInstance = null;

		public override void _Ready()
		{
			base._Ready();

			InputInstance = Input.Singleton;
			MouseSelectRay.ProcessMode = ProcessModeEnum.Disabled;
		}

		private void HandleSelection()
		{
			if (!SelectCooldown.IsStopped())
			{
				return;
			}

			Vector2 direction = InputInstance.GetVector("SelectLeft", "SelectRight", "SelectUp", "SelectDown");
			if (direction.Length() < 0.5f)
			{
				return;
			}

			Vector2[] possibleDirections = { Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right };
			Vector2 closestDirection = possibleDirections.MaxBy(d => direction.Dot(d)).Normalized();

			if (closestDirection.IsEqualApprox(Vector2.Up))
			{
				Interactor.SelectUp();
				SelectCooldown.Start();
			}
			else if (closestDirection.IsEqualApprox(Vector2.Down))
			{
				Interactor.SelectDown();
				SelectCooldown.Start();
			}
			else if (closestDirection.IsEqualApprox(Vector2.Left))
			{
				Interactor.SelectLeft();
				SelectCooldown.Start();
			}
			else if (closestDirection.IsEqualApprox(Vector2.Right))
			{
				Interactor.SelectRight();
				SelectCooldown.Start();
			}
		}

		private void HandleMouseSelection()
		{
			if (!ShouldDoMouseSelection)
			{
				return;
			}

			Camera3D camera = GetViewport().GetCamera3D();
			Vector2 mousePosition = GetViewport().GetMousePosition();
			Vector3 from = camera.ProjectRayOrigin(mousePosition);
			Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1.0f;

			MouseSelectRay.ProcessMode = ProcessModeEnum.Inherit;
			MouseSelectRay.GlobalPosition = from;
			MouseSelectRay.TargetPosition = to;
			MouseSelectRay.ForceRaycastUpdate();
			MouseSelectRay.ProcessMode = ProcessModeEnum.Disabled;

			if (MouseSelectRay.GetCollider() is Interactable interactable)
			{
				Interactor.Selection = interactable;
			}

			ShouldDoMouseSelection = false;
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			HandleSelection();
			HandleMouseSelection();
		}

		public override void _Input(InputEvent inputEvent)
		{
			base._Input(inputEvent);

			if (inputEvent is InputEventMouseMotion mouseMotion)
			{
				ShouldDoMouseSelection = true;
			}
		}
	}
}
