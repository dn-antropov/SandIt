using Godot;

public partial class EconomyManager : Node
{
    [Export]
    public RichTextLabel packetsText;
    [Export]
    public RichTextLabel spamText;
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
        packetsText.Text = packets.ToString();
        spamText.Text = spam.ToString();
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