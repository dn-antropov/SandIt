using Godot;

public partial class SpamEater : Node2D
{
    private int _width = 100, _height = 100;
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

    public override void _Draw()
    {
        // if (Engine.IsEditorHint())
        if (true)
        {
            var rect = new Rect2(new Vector2(0, 0), new Vector2(_width, _height));
            DrawRect(rect, new Color(1, 1, 0, 0.5f), true);
        }
    }
}