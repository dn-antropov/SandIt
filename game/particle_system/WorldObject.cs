using System;
using Godot;

[Tool]
public partial class WorldObject : Node2D
{
    protected Vector2I _gridPosition;
    [Export]
    public Vector2I GridPosition
    {
        get => _gridPosition;
        set
        {
            _gridPosition = value;
            SetPosition((Vector2)_gridPosition * Common.pixelScale);
        }
    }
    protected Vector2I _gridSize = new Vector2I(1, 1);
    [Export]
    public Vector2I GridSize
    {
        get => _gridSize;
        set
        {
            _gridSize = value;
            QueueRedraw();
        }
    }
    protected Color _debugColor = new Color(1, 0, 0, 0.5f);
    [Export]
    public Color DebugColor
    {
        get => _debugColor;
        set
        {
            _debugColor = value;
            QueueRedraw();
        }
    }

    private bool bMoving = false;

    public void Move()
    {
        bMoving = true;
    }

    public virtual void Place()
    {
        bMoving = false;
    }
    public override void _Draw()
    {
        // if (Engine.IsEditorHint())
        if (true)
        {
            var rect = new Rect2(new Vector2(0, 0), (Vector2)_gridSize * Common.pixelScale);
            DrawRect(rect, _debugColor, true);
        }
    }

    public override void _Process(double delta)
    {
        if (bMoving)
        {
            var mousePosition = GetViewport().GetMousePosition();
            mousePosition /= Common.pixelScale;
            mousePosition = mousePosition.Floor();
            mousePosition *= Common.pixelScale;
            SetPosition(mousePosition);
        }
    }

    public bool CanBeBuilt()
    {
        Vector2 scaledPosition = Position / Common.pixelScale;
        return Common.main.IsInBounds((Vector2I)scaledPosition);

    }
}
