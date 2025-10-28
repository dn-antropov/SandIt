using Godot;
using System;

public partial class WallBuildButton : BuildButton
{

    [Export]
    WallEmitter.WallType wallType;

    [Export]
    int _size = 9;
    protected override void OnButtonReleased()
    {
        base.OnButtonReleased();
        WallEmitter wallEmitter = (WallEmitter)worldObject;
        wallEmitter.wallType = wallType;
        wallEmitter.GridSize = new Vector2I(_size, _size);
    }
}
