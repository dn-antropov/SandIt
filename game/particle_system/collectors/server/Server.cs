using Godot;

[Tool]
public partial class Server : WorldObject
{
    private double elapsedTime = 0;

    [Export]
    public double CollectInterval = 1;
    [Export]
    public int PacketsToCollect = 10;

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;

        base._Process(delta);
        elapsedTime += delta;

        if (elapsedTime > CollectInterval)
        {
            CollectNPackets(_gridPosition.X, _gridPosition.Y, _gridSize.X, _gridSize.Y, PacketsToCollect);
            elapsedTime = 0;
        }
    }

    private void CollectNPackets(int x, int y, int width, int height, int N)
    {
        int packets = 0;
        int spam = 0;
        for (int p = 0; p < N; p++)
        {
            Vector2I position = new Vector2I(GD.RandRange(x, x + width), GD.RandRange(y, y + height));
            int type = CollectPacket(position);
            if (type == (int)Common.PacketType.Basic)
            {
                packets++;
            }
            else if (type == (int)Common.PacketType.Spam)
            {
                spam++;
            }
        }
        Common.economy.AddPackets(packets);
        Common.economy.AddSpam(spam);
    }

    private void CollectAllPackets(int x, int y, int width, int height)
    {
        int score = 0;
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
