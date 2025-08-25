using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class WallEmitter : Polygon2D
{
    bool bInitilized = false;
    bool bBuilt = false;
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (bInitilized && !bBuilt)
        {
            BuildTheWall();
        }
        else
        {
            bInitilized = true;
        }
    }

    private void BuildTheWall()
    {
        GD.Print("BuildTheWall");
        for (int px = 0; px < Common.main.GetDimensions().X; px++)
        {
            for (int py = 0; py < Common.main.GetDimensions().Y; py++)
            {
                if (Geometry2D.IsPointInPolygon(new Vector2(py, px) * Common.pixelScale, Polygon))
                {
                    Common.main.Draw(new Vector2I(px, py), (int)Common.PacketType.Wall);
                }
            }
        }
        bBuilt = true;
    }
}
