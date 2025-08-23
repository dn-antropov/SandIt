using Godot;
using System;
using System.Security.Principal;

[Tool]
public partial class RectangularEmitter : Node2D
{
    private int _width, _height = 100;
    private int _x, _y = 0;
    private double elapsedTime = 0;

    [Export]
    public double SpawnInterval = 1;
    [Export]
    public int SpawnAmount = 16;
    [Export]
    public int SpamWeigth = 0;
    [Export]
    public Godot.Collections.Dictionary<Common.PacketType, int> weights;
    [Export]
    public int Width
    {
        get => _width;
        set
        {
            if (_width == value) return;
            _width = value;
            QueueRedraw();
        }
    }

    [Export]
    public int Height
    {
        get => _height;
        set
        {
            if (_height == value) return;
            _height = value;
            QueueRedraw();
        }
    }

    [Export]
    public int X
    {
        get => _x;
        set
        {
            if (_x == value) return;
            _x = value;
            QueueRedraw();
        }
    }

    [Export]
    public int Y
    {
        get => _y;
        set
        {
            if (_y == value) return;
            _y = value;
            QueueRedraw();
        }
    }

    public override void _Draw()
    {
        if (Engine.IsEditorHint())
        {
            var rect = new Rect2(new Vector2(_x, _y), new Vector2(_width, _height));
            DrawRect(rect, new Color(1, 0, 0, 0.5f), true);
        }
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;

        base._Process(delta);
        elapsedTime += delta;
        if (elapsedTime >= SpawnInterval)
        {
            int scaledWidth = (int)(_width / Common.pixelScale);
            int scaledHeight = (int)(_height / Common.pixelScale);
            int scaledX = (int)(_y / Common.pixelScale);
            int scaledY = (int)(_x / Common.pixelScale);
            for (int p = 0; p < SpawnAmount; p++)
            {
                DrawParticle(new Vector2I(GD.RandRange(scaledX, scaledX + scaledHeight), GD.RandRange(scaledY, scaledY + scaledWidth)), (int)WeightedSpawn());
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
