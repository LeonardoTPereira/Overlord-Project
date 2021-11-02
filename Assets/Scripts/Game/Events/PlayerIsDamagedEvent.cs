using System;

public delegate void PlayerIsDamagedEvent(object sender, PlayerIsDamagedEventArgs e);
public class  PlayerIsDamagedEventArgs : EventArgs
{
    private int enemyIndex;
    private int damageDone;
    private int playerHealth;

    public PlayerIsDamagedEventArgs(int enemyIndex, int damageDone, int playerHealth)
    {
        EnemyIndex = enemyIndex;
        DamageDone = damageDone;
        PlayerHealth = playerHealth;
    }

    public int EnemyIndex { get => enemyIndex; set => enemyIndex = value; }
    public int DamageDone { get => damageDone; set => damageDone = value; }
    public int PlayerHealth { get => playerHealth; set => playerHealth = value; }
}
