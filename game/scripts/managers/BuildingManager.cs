using Godot;
using System;

public partial class BuildingManager : Node
{

    public static BuildingManager Instance {get; private set;}
    WorldObject wO;
    public override void _EnterTree()
    {
        Instance = this;
    }
    public WorldObject SpawnWorldObject(PackedScene worldObjectScene)
    {
        // GD.Print("Spawn");
        wO = (WorldObject)worldObjectScene.Instantiate();
        wO.Move();
        AddChild(wO);
        return wO;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouse)
        {
            if (eventMouse.IsReleased())
            {
               wO.Place();
            }
        }
    }
}
