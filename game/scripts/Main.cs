using Godot;
using System;

public partial class Main : Node
{
	int i = 0;
	Variant simulation;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		simulation = ClassDB.Instantiate("GranularSimulation");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Step(1);
	}

	public Vector2I GetDimensions() {
		var dimensions = simulation.AsGodotObject().Call("get_dimensions").AsVector2I();
		return dimensions;
	}

	public byte[] GetRenderData() {
		byte[] renderData = simulation.AsGodotObject().Call("get_render_data").AsByteArray();
		return renderData;
	}

	public bool IsInBounds(Vector2I position) {
		bool isInBounds = simulation.AsGodotObject().Call("is_in_bounds", position.Y, position.X).AsBool();
		return isInBounds;
	}

	public void Draw(Vector2I position) {
		simulation.AsGodotObject().Call("draw_particle", position.Y, position.X, 0);
	}

	public void Step(int iterations) {
		simulation.AsGodotObject().Call("step", iterations);
	}
}
