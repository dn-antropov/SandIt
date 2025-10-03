using Godot;

public partial class XYDestructor : Node
{
    [Export]
    public int pointCollectionCost = 500;
    [Export]
    public int Radius = 3;
    [Export]
    public double Cooldown = 3.0;
    [Signal]
    public delegate void MousePressedEventHandler(Vector2 begin, Vector2 end);

    private double elapsedTime = 0;
    private bool readyToDestroy = true;
    public override void _Ready()
    {
        MousePressed += OnMousePressed;
    }

    public override void _Process(double delta)
    {
        elapsedTime += delta;
        if (IsAllowedToCollect())
        {
            readyToDestroy = true;
            elapsedTime = 0;
        }
        if (Input.IsActionPressed("LeftClick") && readyToDestroy)
        {
            Vector2 begin = GetViewport().GetMousePosition() / Common.pixelScale;
            EmitSignal(SignalName.MousePressed, begin, new Vector2(1, 1));

            readyToDestroy = false;
            elapsedTime = 0;
        }
    }

    public void OnMousePressed(Vector2 begin, Vector2 end) {
        DestoyPacketsCircular(begin, Radius);
    }

    private void DestoyPacketsCircular(Vector2 position, int radius) {
        Vector2I position_i = new Vector2I((int)position.Y, (int)position.X);
        if (Common.main.IsInBounds(position_i)) {
            for (int row = -radius; row <= radius + 1; row++) {
                for (int col = -radius; col <= radius + 1; col++) {
                    if (row * row + col * col < radius * radius)
                    {
                        Vector2I Position = new Vector2I(row + position_i.X, col + position_i.Y);
                        DestroyPackets(Position);
                    }
                }
            }
        }
    }

    private void DestroyPackets(Vector2I position) {
        Common.main.Erase(position);
    }

    private bool IsAllowedToCollect()
    {
        return elapsedTime >= Cooldown &&
               !readyToDestroy &&
               (Common.economy.Packets >= pointCollectionCost);
    }
}