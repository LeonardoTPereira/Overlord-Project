using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;

public delegate void LevelLoadEvent(object sender, LevelLoadEventArgs e);
public class LevelLoadEventArgs : EventArgs
{
    private string _levelFile;
    private DungeonFileSo _dungeonFileSo;
    private QuestLine _levelQuestLine;

    public QuestLine LevelQuestLine
    {
        get => _levelQuestLine;
        set => _levelQuestLine = value;
    }


    public LevelLoadEventArgs(DungeonFileSo dungeonFileSo, QuestLine levelQuestLine)
    {
        _levelFile = null;
        DungeonFileSo = dungeonFileSo;
        LevelQuestLine = levelQuestLine;
    }

    public LevelLoadEventArgs(string levelFile)
    {
        LevelFile = levelFile;
        DungeonFileSo = null;
        LevelQuestLine = null;
    }

    public string LevelFile { get => _levelFile; set => _levelFile = value; }
    public DungeonFileSo DungeonFileSo { get => _dungeonFileSo; set => _dungeonFileSo = value; }
}