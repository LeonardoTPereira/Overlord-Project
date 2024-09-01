using System;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.Quests;

namespace Game.Events
{
    public delegate void LevelLoadEvent(object sender, LevelLoadEventArgs e);
    public class LevelLoadEventArgs : EventArgs
    {
        private string _levelFile;
        private DungeonFileSo _dungeonFileSo;
        public QuestLineList LevelQuestLines { get; set; }
        private bool _isLastQuestLine;


        public LevelLoadEventArgs(DungeonFileSo dungeonFileSo, QuestLineList levelQuestLines, bool isLastQuestLine)
        {
            _levelFile = null;
            DungeonFileSo = dungeonFileSo;
            LevelQuestLines = levelQuestLines;
            IsLastQuestLine = isLastQuestLine;
        }

        public LevelLoadEventArgs(string levelFile)
        {
            LevelFile = levelFile;
            DungeonFileSo = null;
            LevelQuestLines = null;
            _isLastQuestLine = false;
        }

        public string LevelFile { get => _levelFile; set => _levelFile = value; }
        public DungeonFileSo DungeonFileSo { get => _dungeonFileSo; set => _dungeonFileSo = value; }
        public bool IsLastQuestLine
        {
            get => _isLastQuestLine;
            set => _isLastQuestLine = value;
        }
    }
}