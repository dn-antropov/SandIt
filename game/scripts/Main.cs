using Godot;
using Godot.NativeInterop;
using System;
using System.Linq;




public partial class Main : Node
{
	int counter = 0;
	Variant simulation;

	PackedScene sandPileCollision = GD.Load<PackedScene>("res://objects/sand_pile_collision.tscn");
	Godot.Collections.Array<Node> sandPileCollisionInstances = [];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		simulation = ClassDB.Instantiate("GranularSimulation");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Step(1);
		// outline = simulation.AsGodotObject().Call("get_outline").AsVector2Array();
		Godot.Collections.Array<Vector2[]> outlines = simulation.AsGodotObject().Call("get_outlines").AsGodotArray<Vector2[]>();
		Godot.Collections.Array<Vector2[]> simplified_outlines = simulation.AsGodotObject().Call("get_simplified_outlines").AsGodotArray<Vector2[]>();

		for (int i = 0; i < sandPileCollisionInstances.Count; i++)
		{
			sandPileCollisionInstances[i].QueueFree();
		}
		sandPileCollisionInstances.Clear();
		for (int i = 0; i < outlines.Count; i++)
		{
			Node instance = sandPileCollision.Instantiate();
			instance.GetNode<Line2D>("Outline_Pile").Points = MapOutline(outlines[i]);
			instance.GetNode<Line2D>("Outline_Collision").Points = MapOutline(simplified_outlines[i]);
			instance.GetNode<CollisionPolygon2D>("StaticBody/CollisionPolygon2D").Polygon = MapOutline(simplified_outlines[i]);
			AddChild(instance);
			sandPileCollisionInstances.Add(instance);
			GD.Print(sandPileCollisionInstances.Count);
		}
	}

	Vector2[] MapOutline(Vector2[] outline) {
		Vector2[] mappedOutline = new Vector2[outline.Length];
		int i = 0;
		foreach (Vector2 position in outline) {
			mappedOutline[i] = new Vector2(position.Y * Common.pixelScale, position.X * Common.pixelScale);
			i++;
		}
		return mappedOutline;
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

	public void Draw(Vector2I position) {
		simulation.AsGodotObject().Call("draw_particle", position.X, position.Y, 0);
	}

	public void Step(int iterations) {
		simulation.AsGodotObject().Call("step", iterations);
	}
}
