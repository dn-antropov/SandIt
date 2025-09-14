using Godot;

[Tool]
public partial class Server : Node2D
{
    private int _width, _height = 100;

    // private int _x, _y = 0;

    private double elapsedTime = 0;

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

    public override void _Draw()
    {
        // if (Engine.IsEditorHint())
        if (true)
        {
            var rect = new Rect2(new Vector2(0, 0), new Vector2(_width, _height));
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
            int scaledX = (int)(Position.Y / Common.pixelScale);
            int scaledY = (int)(Position.X / Common.pixelScale);
            CollectNPackets(scaledX, scaledY, scaledWidth, scaledHeight, PacketsToCollect);
            //CollectAllPackets(scaledX, scaledY, scaledWidth, scaledHeight);
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
