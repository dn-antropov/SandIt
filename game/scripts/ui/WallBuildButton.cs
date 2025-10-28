using Godot;
using System;

public partial class WallBuildButton : BuildButton
{

    [Export]
    WallEmitter.WallType wallType;
    protected override void OnButtonReleased()
    {
        base.OnButtonReleased();
        WallEmitter wallEmitter = (WallEmitter)worldObject;
        wallEmitter.wallType = wallType;
    }
}
