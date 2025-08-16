using Godot;

public partial class Common : Node
{
    public static Main main;
    public static RenderSim renderSim;

    // 1080/256
    public static float pixelScale = 4.21875F;

    public override void _Ready()
	{
		main = GetTree().Root.GetNode<Main>("Main");
        renderSim = GetNode<RenderSim>("/root/Main/RenderSim");
	}
}
