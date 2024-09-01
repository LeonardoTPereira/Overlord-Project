namespace Game.LevelSelection
{
    public class RealTimeLevelDescription : LevelDescription
    {
        public override void CreateDescriptions(LevelData levelData)
        {
            if (levelData is not RealTimeLevelData realTimeLevelData) return;
            CreateDungeonDescription(realTimeLevelData);
            _isShowingDungeon = false;
            ChangeDescription();
        }

        private void CreateDungeonDescription(RealTimeLevelData realTimeLevelData)
        {
            if (realTimeLevelData.AchievementWeight == 4)
            {
                DungeonDescription = "A dungeon for Achievers";
            }
            else if (realTimeLevelData.CreativityWeight == 4)
            {
                DungeonDescription = "A dungeon for Explorers";
            }
            else if (realTimeLevelData.ImmersionWeight == 4)
            {
                DungeonDescription = "A dungeon for those that like to immerse themselves";
            }
            else if (realTimeLevelData.MasteryWeight == 4)
            {
                DungeonDescription = "A dungeon for those that like combat";
            }
        }
    }
}