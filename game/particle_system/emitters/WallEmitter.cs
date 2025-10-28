using System;
using Godot;

[Tool]
public partial class WallEmitter: WorldObject
{
    public enum WallType
    {
        Vertical,
        Horizontal,
        DiagonalAsc,
        DiagonalDes
    }

    [Export]
    public WallType wallType;

    bool bInitilized = false;
    bool bBuilt = false;
    public override void _Process(double delta)
    {
        base._Process(delta);
        // if (bInitilized && !bBuilt)
        // {
        //     BuildTheWall();
        // }
        // else
        // {
        //     bInitilized = true;
        // }
    }

    public override void _Draw()
    {
        // base._Draw();
        switch (wallType)
        {
            case WallType.Vertical:
                DrawRect(new Rect2(new Vector2(0, 0), new Vector2(1, Math.Max(_gridSize.X, _gridSize.Y)) * Common.pixelScale), _debugColor, true);
                break;
            case WallType.Horizontal:
                DrawRect(new Rect2(new Vector2(0, 0), new Vector2(Math.Max(_gridSize.X, _gridSize.Y), 1) * Common.pixelScale), _debugColor, true);
                break;
            case WallType.DiagonalAsc:
                for (int step = 0; step < Math.Max(_gridSize.X, _gridSize.Y); step++)
                {
                    var rect = new Rect2(new Vector2(step, Math.Max(_gridSize.X, _gridSize.Y) - 1 - step) * Common.pixelScale, new Vector2(1, 1) * Common.pixelScale);
                    DrawRect(rect, _debugColor, true);
                    if (step < Math.Max(_gridSize.X, _gridSize.Y) - 1)
                    {
                        var fillerRect = new Rect2(new Vector2(step + 1, Math.Max(_gridSize.X, _gridSize.Y) - 1 - step) * Common.pixelScale, new Vector2(1, 1) * Common.pixelScale);
                        DrawRect(fillerRect, _debugColor, true);
                    }
                }
                break;
            case WallType.DiagonalDes:
                for (int step = 0; step < Math.Max(_gridSize.X, _gridSize.Y); step++)
                {
                    var rect = new Rect2(new Vector2(step, step) * Common.pixelScale, new Vector2(1, 1) * Common.pixelScale);
                    DrawRect(rect, _debugColor, true);
                    if (step < Math.Max(_gridSize.X, _gridSize.Y) - 1)
                    {
                        var fillerRect = new Rect2(new Vector2(step, step + 1) * Common.pixelScale, new Vector2(1, 1) * Common.pixelScale);
                        DrawRect(fillerRect, _debugColor, true);
                    }
                }
                break;
        }
    }

    public override void Place()
    {
        base.Place();
        BuildTheWall();
    }


    private void BuildTheWall()
    {
        switch(wallType)
        {
            case WallType.DiagonalAsc:
                for (int step = 0; step < Math.Max(_gridSize.X, _gridSize.Y); step++)
                {
                    Vector2I pos = new Vector2I(step, Math.Max(_gridSize.X, _gridSize.Y) - 1 - step) + (Vector2I)(Position / Common.pixelScale);
                    Common.main.Draw(pos, (int)Common.PacketType.Wall);
                    if (step < Math.Max(_gridSize.X, _gridSize.Y) - 1)
                    {
                        Vector2I fillerPos = new Vector2I(step + 1, Math.Max(_gridSize.X, _gridSize.Y) - 1 - step) + (Vector2I)(Position / Common.pixelScale);
                        Common.main.Draw(fillerPos, (int)Common.PacketType.Wall);
                    }
                }
                break;
            case WallType.DiagonalDes:
                for (int step = 0; step < Math.Max(_gridSize.X, _gridSize.Y); step++)
                {
                    Vector2I pos = new Vector2I(step, step) + (Vector2I)(Position / Common.pixelScale);
                    Common.main.Draw(pos, (int)Common.PacketType.Wall);
                    if (step < Math.Max(_gridSize.X, _gridSize.Y) - 1)
                    {
                        Vector2I fillerPos = new Vector2I(step, step + 1) + (Vector2I)(Position / Common.pixelScale);
                        Common.main.Draw(fillerPos, (int)Common.PacketType.Wall);
                    }
                }
                break;
        }
        bBuilt = true;
    }
}
