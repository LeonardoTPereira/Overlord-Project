using System;

public delegate void LevelLoadEvent(object sender, LevelLoadEventArgs e);
public class LevelLoadEventArgs : EventArgs
{
    private string levelFile;
    private DungeonFileSO dungeonFileSO;

    public LevelLoadEventArgs(DungeonFileSO dungeonFileSO)
    {
        DungeonFileSO = dungeonFileSO;
    }

    public LevelLoadEventArgs(string levelFile)
    {
        LevelFile = levelFile;
    }

    public string LevelFile { get => levelFile; set => levelFile = value; }
    public DungeonFileSO DungeonFileSO { get => dungeonFileSO; set => dungeonFileSO = value; }
}