using Godot;

public partial class EconomyManager : Node
{
    [Export]
    public RichTextLabel balance;
    public RichTextLabel harm;
    private int packets = 0;

    private int spam = 0;

    public int Packets
    {
        get => packets;
        set
        {
            return;
        }
    }
    public int Spam
    {
        get => spam;
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
        balance.Text = packets.ToString();
        harm.Text = spam.ToString();
    }

    public void AddPackets(int numPackets)
    {
        packets += numPackets;
    }

    public void AddSpam(int numPackets)
    {
        spam += numPackets;
    }
}