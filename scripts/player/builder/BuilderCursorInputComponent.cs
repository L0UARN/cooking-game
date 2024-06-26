using System.Linq;
using Godot;
using Godot.Collections;

namespace CookingGame
{
	[GlobalClass]
	public partial class BuilderCursorInputComponent : Node
	{
		[Export]
		private BuilderCursorComponent Cursor = null;
		[Export]
		private Timer NavigationCooldown = null;
		[Export]
		private Timer BuildCooldown = null;
		[Export]
		private Node3D CameraPivot = null;
		[Export]
		private Timer ViewCooldown = null;
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

			Vector2 inputDirection = InputInstance.GetVector("BuilderNavigateLeft", "BuilderNavigateRight", "BuilderNavigateUp", "BuilderNavigateDown");
			if (inputDirection.Length() < 0.5f)
			{
				return;
			}

			Array<Vector2> possibleDirections = new() { Vector2.Up, Vector2.Left, Vector2.Down, Vector2.Right };
			Vector2 closestDirection = possibleDirections.MaxBy((Vector2 vector) => vector.Dot(inputDirection));
			Vector2 adjustedDirection = closestDirection.Rotated(-CameraPivot.Rotation.Y);

			if (adjustedDirection.IsEqualApprox(Vector2.Up))
			{
				Cursor.NavigateUp();
				NavigationCooldown.Start();
			}
			else if (adjustedDirection.IsEqualApprox(Vector2.Left))
			{
				Cursor.NavigateLeft();
				NavigationCooldown.Start();
			}
			else if (adjustedDirection.IsEqualApprox(Vector2.Down))
			{
				Cursor.NavigateDown();
				NavigationCooldown.Start();
			}
			else if (adjustedDirection.IsEqualApprox(Vector2.Right))
			{
				Cursor.NavigateRight();
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
			Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 500.0f;

			MouseNavigationRay.ProcessMode = ProcessModeEnum.Inherit;
			MouseNavigationRay.GlobalPosition = from;
			MouseNavigationRay.TargetPosition = to;
			MouseNavigationRay.ForceRaycastUpdate();
			MouseNavigationRay.ProcessMode = ProcessModeEnum.Disabled;

			if (MouseNavigationRay.GetCollider() is GridTile tile)
			{
				Cursor.NavigateToTile(tile);
			}
			else if (MouseNavigationRay.GetCollider() is BuildableComponent buildable)
			{
				from = buildable.GlobalPosition;
				to = from + Vector3.Down * 2.0f;

				MouseNavigationRay.ProcessMode = ProcessModeEnum.Inherit;
				MouseNavigationRay.GlobalPosition = from;
				MouseNavigationRay.TargetPosition = to;
				MouseNavigationRay.ForceRaycastUpdate();
				MouseNavigationRay.ProcessMode = ProcessModeEnum.Disabled;

				if (MouseNavigationRay.GetCollider() is GridTile tileUnderBuildable)
				{
					Cursor.NavigateToTile(tileUnderBuildable);
				}
			}

			ShouldDoMouseNavigation = false;
		}

		private void HandleBuild()
		{
			if (!BuildCooldown.IsStopped())
			{
				return;
			}

			if (InputInstance.GetActionStrength("BuilderBuild") >= 0.5f)
			{
				Cursor.Build();
				BuildCooldown.Start();
			}
			else if (InputInstance.GetActionStrength("BuilderDestroy") >= 0.5f)
			{
				Cursor.Destroy();
				BuildCooldown.Start();
			}
			else if (InputInstance.GetActionStrength("BuilderRotate") >= 0.5f)
			{
				Cursor.Rotate();
				BuildCooldown.Start();
			}
		}

		private void HandleView()
		{
			if (!ViewCooldown.IsStopped())
			{
				return;
			}

			if (InputInstance.GetActionStrength("BuilderViewLeft") >= 0.5f)
			{
				CameraPivot.Rotate(Vector3.Up, -Mathf.Pi * 0.5f);
				ViewCooldown.Start();
			}
			else if (InputInstance.GetActionStrength("BuilderViewRight") >= 0.5f)
			{
				CameraPivot.Rotate(Vector3.Up, Mathf.Pi * 0.5f);
				ViewCooldown.Start();
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			HandleNavigation();
			HandleMouseNavigation();
			HandleView();
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
