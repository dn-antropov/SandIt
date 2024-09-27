using Godot;

public partial class Common : Node
{
    public static Main main;
    public static RenderSim renderSim;

    public static int pixelScale = 4;

    public override void _Ready()
	{
		main = GetTree().Root.GetNode<Main>("Main");
        renderSim = GetNode<RenderSim>("/root/Main/RenderSim");
	}
}
