namespace Game.LevelSelection
{
    public class PortugueseRealTimeLevelDescription : RealTimeLevelDescription
    {
        protected override void CreateDungeonDescription(RealTimeLevelData realTimeLevelData)
        {
            if (realTimeLevelData.AchievementWeight == 4)
            {
                DungeonDescription = "Para pessoas que gostam de completar conquistas e se tornar poderoso.";
            }
            else if (realTimeLevelData.CreativityWeight == 4)
            {
                DungeonDescription = "Para pessoas que gostam de exploração.";
            }
            else if (realTimeLevelData.ImmersionWeight == 4)
            {
                DungeonDescription = "Para pessoas que valorizam a imersão.";
            }
            else if (realTimeLevelData.MasteryWeight == 4)
            {
                DungeonDescription = "Para pessoas que gostam de combate.";
            }
        }
    }
}