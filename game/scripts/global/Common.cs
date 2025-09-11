using Godot;

public partial class Common : Node
{
    public enum PacketType
    {
        Nothing,
        Wall,
        Basic,
        Spam
    }
    public static Main main;
    public static RenderSim renderSim;
    public static EconomyManager economy;

    public static float pixelScale = 16F;

    public override void _Ready()
	{
		main = GetTree().Root.GetNode<Main>("Main");
        renderSim = GetNode<RenderSim>("/root/Main/RenderSim");
        economy = GetNode<EconomyManager>("/root/Main/Economy");
	}
}
