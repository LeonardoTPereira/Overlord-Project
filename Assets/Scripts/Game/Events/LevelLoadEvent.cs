using System;

public delegate void LevelLoadEvent(object sender, LevelLoadEventArgs e);
public class LevelLoadEventArgs : EventArgs
{
    private string levelFile;

    public LevelLoadEventArgs(string levelFile)
    {
        LevelFile = levelFile;
    }

    public string LevelFile { get => levelFile; set => levelFile = value; }
}