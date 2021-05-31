using System;

public delegate void LevelLoadEvent(object sender, LevelLoadEventArgs e);
public class LevelLoadEventArgs : EventArgs
{
    private string levelFile, enemyFile;

    public LevelLoadEventArgs(string levelFile, string enemyFile)
    {
        LevelFile = levelFile;
        EnemyFile = enemyFile;
    }

    public string LevelFile { get => levelFile; set => levelFile = value; }
    public string EnemyFile { get => enemyFile; set => enemyFile = value; }
}