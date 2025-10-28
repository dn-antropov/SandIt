using Godot;
using System;

[Tool]
public partial class PacketsEmitter : WorldObject
{
    private double elapsedTime = 0;

    [Export]
    public double SpawnInterval = 1;
    [Export]
    public int SpawnAmount = 16;
    [Export]
    public int SpamWeigth = 0;
    [Export]
    public Godot.Collections.Dictionary<Common.PacketType, int> weights;

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;

        base._Process(delta);
        elapsedTime += delta;
        if (elapsedTime >= SpawnInterval)
        {

            for (int p = 0; p < SpawnAmount; p++)
            {
                DrawParticle(new Vector2I(GD.RandRange(_gridPosition.X, _gridPosition.X + _gridSize.X),
                                          GD.RandRange(_gridPosition.Y, _gridPosition.Y + _gridSize.Y)), (int)WeightedSpawn());
            }
            elapsedTime = 0;
        }
    }

    private Common.PacketType WeightedSpawn()
    {
        int totalWeights = 0;
        foreach (int weight in weights.Values)
        {
            totalWeights += weight;
        }
        double wRandom = Math.Ceiling(GD.Randf() * totalWeights);
        foreach (Common.PacketType packetType in weights.Keys)
        {
            if (wRandom < weights[packetType])
            {
                return packetType;
            }
            wRandom -= weights[packetType];
        }
        return Common.PacketType.Nothing;
    }

    private void DrawParticle(Vector2I position, int type)
    {
        Common.main.Draw(position, type);
    }
}
