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
    public int SpawnAmount = 100;

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
            for (int p = 0; p < SpawnAmount; p++)
            {
                int scaledWidth = (int)(_width / Common.pixelScale);
                int scaledHeight = (int)(_height / Common.pixelScale);
                DrawParticle(new Vector2I(GD.RandRange(0, scaledHeight), GD.RandRange(0, scaledWidth)));
            }
            elapsedTime = 0;
        }
    }


    private void DrawParticle(Vector2I position)
    {
        Common.main.Draw(position);
    }
}
