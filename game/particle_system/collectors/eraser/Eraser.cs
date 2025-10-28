using Godot;

[Tool]
public partial class Eraser : WorldObject
{
    private double elapsedTime = 0;
    // Seconds
    // [Export]
    // private double CollectInterval = Common.main.timestep;

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;

        base._Process(delta);

        if (Common.main == null)
            return;

        elapsedTime += delta;
        if (elapsedTime > Common.main.timestep)
        {
            ErasePackets(_gridPosition.X, _gridPosition.Y, _gridSize.X, _gridSize.Y);
            elapsedTime = 0;
        }
    }

    private void ErasePackets(int x, int y, int width, int height)
    {
        for (int px = x; px < (x + width); px++)
        {
            for (int py = y; py < (y + height); py++)
            {
                int type = ErasePacket(new Vector2I(px, py));
            }
        }
    }

    private int ErasePacket(Vector2I position)
    {
        return Common.main.Erase(position);
    }

}