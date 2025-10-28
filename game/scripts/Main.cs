using Godot;

public partial class Main : Node
{
	[Signal]
	public delegate void UpdateSimEventHandler();

	Variant simulation;

	double elapsedTime = 0;

	[Export]
	public double timestep = 0.5;

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

	public byte[] GetPreviosRenderData() {
		byte[] renderData = simulation.AsGodotObject().Call("get_previous_render_data").AsByteArray();
		return renderData;
	}

	public Vector4[] GetInterpolatedRenderData(float alpha = 0.0f, int renderScale = 16) {
		Vector4[] renderData = simulation.AsGodotObject().Call("get_interpolated_render_data", alpha, renderScale).AsVector4Array();
		return renderData;
	}

	public bool IsInBounds(Vector2I position)
	{
		bool isInBounds = simulation.AsGodotObject().Call("is_in_bounds", position.Y, position.X).AsBool();
		return isInBounds;
	}

	public void Draw(Vector2I position, int type) {
		simulation.AsGodotObject().Call("create_particle", position.Y, position.X, type);
	}

	public int Erase(Vector2I position)
	{
		int type = simulation.AsGodotObject().Call("destroy_particle", position.Y, position.X).AsInt32();
		return type;
	}

	public void Step(int iterations)
	{
		simulation.AsGodotObject().Call("step", iterations);
	}

	public int GetPacketType(Vector2I pos)
    {
		return simulation.AsGodotObject().Call("get_packet_type").AsInt32();
    }
}
