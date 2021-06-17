using System;

public delegate void TreasureCollectEvent(object sender, TreasureCollectEventArgs e);

public class TreasureCollectEventArgs : EventArgs
{
    private int amount;

    public TreasureCollectEventArgs(int amount)
    {
        Amount = amount;
    }
    public int Amount { get => amount; set => amount = value; }
}
