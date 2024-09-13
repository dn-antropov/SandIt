using Godot;
using System;

public partial class Painter : Node
{
	[Signal]
	public delegate void MousePressedEventHandler(Vector2 begin, Vector2 end);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MousePressed += OnMousePressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("LeftClick")) {
			Vector2 begin = GetViewport().GetMousePosition() / Common.pixelScale;
			EmitSignal(SignalName.MousePressed, begin, new Vector2(1,1));
		}
	}

	public void OnMousePressed(Vector2 begin, Vector2 end) {
		DrawCircle(begin, 2);
	}

	private void DrawCircle(Vector2 position, int radius) {
		Vector2I position_i = new Vector2I((int)position.X, (int)position.Y);
		if (Common.main.IsInBounds(position_i)) {
			for (int row = -radius; row <= radius + 1; row++) {
				for (int col = -radius; col <= radius + 1; col++) {
					if (row*row + col*col < radius*radius) {
						Vector2I drawPosition = new Vector2I(row + position_i.X, col + position_i.Y);
						DrawPixel(drawPosition);
					}
				}
			}
		}
	}

	private void DrawPixel(Vector2I position) {
		Common.main.Draw(position);
	}
}
