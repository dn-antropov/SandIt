using Godot;

[Tool]
public partial class Server : WorldObject
{
    private double elapsedTime = 0;

    [Export]
    public double CollectInterval = 1;
    [Export]
    public int PacketsToCollect = 10;

    public override void _Ready()
    {
        base._Ready();
        CallDeferred("BuildTheWalls");
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;

        base._Process(delta);
        elapsedTime += delta;

        if (elapsedTime > CollectInterval)
        {
            // Take care about not collecting walls
            CollectNPackets(_gridPosition.X + 1, _gridPosition.Y, _gridSize.X - 2, _gridSize.Y - 1, PacketsToCollect);
            elapsedTime = 0;
        }
    }

    private void CollectNPackets(int x, int y, int width, int height, int N)
    {
        int packets = 0;
        int spam = 0;
        for (int p = 0; p < N; p++)
        {
            Vector2I position = new Vector2I(GD.RandRange(x, x + width - 1), GD.RandRange(y, y + height - 1));
            // if ((Common.PacketType)Common.main.GetPacketType(position) == Common.PacketType.Wall)
            //     return;
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

    private void BuildTheWalls()
    {
        for (int y = 0; y < _gridSize.Y; y++)
        {
            Vector2I pos = new Vector2I(_gridPosition.X, _gridPosition.Y + y);
            Common.main.Draw(pos, (int)Common.PacketType.Wall);

            pos = new Vector2I(_gridPosition.X + _gridSize.X - 1, _gridPosition.Y + y);
            Common.main.Draw(pos, (int)Common.PacketType.Wall);
        }
        for (int x = 1; x < _gridSize.X - 1; x++)
        {
            Vector2I pos = new Vector2I(_gridPosition.X + x, _gridPosition.Y + _gridSize.Y - 1);
            Common.main.Draw(pos, (int)Common.PacketType.Wall);
        }
    }
}
