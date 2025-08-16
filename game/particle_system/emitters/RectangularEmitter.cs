using Godot;
using System;

[Tool]
public partial class RectangularEmitter : Node2D
{
    private int _width, _height = 100;

    private int _x, _y = 0;

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
        var rect = new Rect2(new Vector2(_x, _y), new Vector2(_width, _height));
        DrawRect(rect, new Color(1, 0, 0, 0.5f), true);
        DrawRect(rect, Colors.Red, false); // outline
    }
}
