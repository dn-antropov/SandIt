using Godot;
using System;

public partial class BuildButton : TextureButton
{

    [Export]
    PackedScene worldObjectToBuild;

    protected WorldObject worldObject;

    public override void _Ready()
    {
        this.ButtonUp += OnButtonReleased;
    }

    protected virtual void OnButtonReleased()
    {
        worldObject = BuildingManager.Instance.SpawnWorldObject(worldObjectToBuild);
    }
}
