using Godot;

public partial class EconomyManager : Node
{
    [Export]
    public RichTextLabel score;
    private int packetsConsumed = 0;

    public int PacketsConsumed
    {
        get => packetsConsumed;
        set
        {
            return;
        }
    }
    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        score.Text = packetsConsumed.ToString();
    }

    public void AddPacketsConsumed(int numPackets)
    {
        packetsConsumed += numPackets;
    }
}