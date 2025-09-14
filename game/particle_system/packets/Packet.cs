using Godot;
using Godot.Collections;

// using System.Collections.Generic;

public partial class Packet : Sprite2D
{
    [Export] public Dictionary<Common.PacketType, Texture2D> textures;

    Common.PacketType packetType;

    public void SetType(int type)
    {
        if (packetType == (Common.PacketType)type)
            return;

        packetType = (Common.PacketType)type;
        textures.TryGetValue(packetType, out Texture2D _texture);
        Texture = _texture;
        if (packetType == Common.PacketType.Spam)
        {
            Modulate = new Color(1, 0, 0, 1);
        }
        else
        {
            Modulate = new Color(1, 1, 1, 1);
        }
    }
}
