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
        private QuestLine _levelQuestLine;
        private bool _isLastQuestLine;


        public LevelLoadEventArgs(DungeonFileSo dungeonFileSo, QuestLine levelQuestLine, bool isLastQuestLine)
        {
            _levelFile = null;
            DungeonFileSo = dungeonFileSo;
            LevelQuestLine = levelQuestLine;
            IsLastQuestLine = isLastQuestLine;
        }

        public LevelLoadEventArgs(string levelFile)
        {
            LevelFile = levelFile;
            DungeonFileSo = null;
            LevelQuestLine = null;
            _isLastQuestLine = false;
        }

        public string LevelFile { get => _levelFile; set => _levelFile = value; }
        public DungeonFileSo DungeonFileSo { get => _dungeonFileSo; set => _dungeonFileSo = value; }
        public bool IsLastQuestLine
        {
            get => _isLastQuestLine;
            set => _isLastQuestLine = value;
        }
        public QuestLine LevelQuestLine
        {
            get => _levelQuestLine;
            set => _levelQuestLine = value;
        }
    }
}