using Godot;

public partial class Main : Node
{
	[Signal]
	public delegate void UpdateSimEventHandler();

	Variant simulation;

	double elapsedTime = 0;

	public double timestep = 0.066;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		simulation = ClassDB.Instantiate("GranularSimulation");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		elapsedTime += delta;
		if (elapsedTime > timestep)
		{
			Step(1);
			EmitSignal(SignalName.UpdateSim);
			elapsedTime = 0;
		}
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
		bool isInBounds = simulation.AsGodotObject().Call("is_in_bounds", position.X, position.Y).AsBool();
		return isInBounds;
	}

	public void Draw(Vector2I position, int type) {
		simulation.AsGodotObject().Call("create_particle", position.X, position.Y, type);
	}

	public int Erase(Vector2I position)
	{
		int type = simulation.AsGodotObject().Call("destroy_particle", position.X, position.Y).AsInt32();
		return type;
	}

	public void Step(int iterations)
	{
		simulation.AsGodotObject().Call("step", iterations);
	}
}
