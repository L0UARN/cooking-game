using System.Linq;
using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuilderCursorInputComponent : Node
	{
		[Export]
		private GridNavigatorComponent Navigator = null;
		[Export]
		private Timer NavigationCooldown = null;
		[Export]
		private BuilderCursorComponent Cursor = null;
		[Export]
		private Timer BuildCooldown = null;
		[Export]
		private BuilderInventoryComponent Inventory = null;
		[Export]
		private RayCast3D MouseNavigationRay = null;

		private InputInstance InputInstance = null;
		private bool ShouldDoMouseNavigation = false;

		public override void _Ready()
		{
			base._Ready();

			InputInstance = Input.Singleton;
			MouseNavigationRay.TopLevel = true;
		}

		private void HandleNavigation()
		{
			if (!NavigationCooldown.IsStopped())
			{
				return;
			}

			Vector2 inputDirection = InputInstance.GetVector("NavigateLeft", "NavigateRight", "NavigateUp", "NavigateDown");
			if (inputDirection.Length() < 0.5f)
			{
				return;
			}

			Array<Vector2> possibleDirections = new() { Vector2.Up, Vector2.Left, Vector2.Down, Vector2.Right };
			Vector2 closestDirection = possibleDirections.MaxBy((Vector2 vector) => vector.Dot(inputDirection));

			if (closestDirection.IsEqualApprox(Vector2.Up))
			{
				Navigator.NavigateUp();
				NavigationCooldown.Start();
			}
			else if (closestDirection.IsEqualApprox(Vector2.Left))
			{
				Navigator.NavigateLeft();
				NavigationCooldown.Start();
			}
			else if (closestDirection.IsEqualApprox(Vector2.Down))
			{
				Navigator.NavigateDown();
				NavigationCooldown.Start();
			}
			else if (closestDirection.IsEqualApprox(Vector2.Right))
			{
				Navigator.NavigateRight();
				NavigationCooldown.Start();
			}
		}

		private void HandleMouseNavigation()
		{
			if (!ShouldDoMouseNavigation)
			{
				return;
			}

			Vector2 mousePosition = GetViewport().GetMousePosition();
			Camera3D camera = GetViewport().GetCamera3D();
			Vector3 from = camera.ProjectRayOrigin(mousePosition);
			Vector3 to = Vector3.Down * 10.0f;

			MouseNavigationRay.ProcessMode = ProcessModeEnum.Inherit;
			MouseNavigationRay.GlobalPosition = from;
			MouseNavigationRay.TargetPosition = to;
			MouseNavigationRay.ForceRaycastUpdate();

			if (MouseNavigationRay.GetCollider() is GridTile tile)
			{
				Navigator.MoveTo(tile);
			}

			MouseNavigationRay.ProcessMode = ProcessModeEnum.Disabled;
			ShouldDoMouseNavigation = false;
		}

		private void HandleBuild()
		{
			if (!BuildCooldown.IsStopped())
			{
				return;
			}

			if (InputInstance.GetActionStrength("Build") >= 0.5f)
			{
				Cursor.Build();
				BuildCooldown.Start();
			}
			else if (InputInstance.GetActionStrength("Destroy") >= 0.5f)
			{
				Cursor.Destroy();
				BuildCooldown.Start();
			}
			else if (InputInstance.GetActionStrength("Rotate") >= 0.5f)
			{
				Cursor.Rotate();
				BuildCooldown.Start();
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			HandleNavigation();
			HandleMouseNavigation();
			HandleBuild();
		}

		public override void _Input(InputEvent inputEvent)
		{
			base._Input(inputEvent);

			if (inputEvent is InputEventMouseMotion mouseMotion)
			{
				ShouldDoMouseNavigation = true;
			}
		}
	}
}
