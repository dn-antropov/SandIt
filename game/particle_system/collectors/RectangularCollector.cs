using Godot;
using System;
using System.Security.Principal;

[Tool]
public partial class RectangularCollector : Node2D
{
    private int _width, _height = 100;

    private int _x, _y = 0;

    private double elapsedTime = 0;
    private int score = 0;

    [Export]
    public double CollectInterval = 1;
    [Export]
    public int PacketsToCollect = 0;

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
            DrawRect(rect, new Color(0, 1, 0, 0.5f), true);
        }
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;
        base._Process(delta);
        elapsedTime += delta;

        if (elapsedTime > CollectInterval)
        {
            int scaledWidth = (int)(_width / Common.pixelScale);
            int scaledHeight = (int)(_height / Common.pixelScale);
            int scaledX = (int)(_y / Common.pixelScale);
            int scaledY = (int)(_x / Common.pixelScale);
            CollectNPackets(scaledX, scaledY, scaledWidth, scaledHeight, PacketsToCollect);
            //CollectAllPackets(scaledX, scaledY, scaledWidth, scaledHeight);
            elapsedTime = 0;
        }
    }

    private void CollectNPackets(int x, int y, int width, int height, int N)
    {
        for (int p = 0; p < N; p++)
        {
            Vector2I position = new Vector2I(GD.RandRange(x, x + width), GD.RandRange(y, y + height));
            int type = CollectPacket(position);
            if (type == (int)Common.PacketType.Basic)
            {
                score++;
            }
        }
    }

    private void CollectAllPackets(int x, int y, int width, int height)
    {
        for (int px = x; px < (x + width); px++)
        {
            for (int py = y; py < (y + height); py++)
            {
                int type = CollectPacket(new Vector2I(px, py));
                if (type == (int)Common.PacketType.Basic)
                {
                    score++;
                }
            }
        }
    }
    private int CollectPacket(Vector2I position)
    {
        return Common.main.Erase(position);
    }
}
