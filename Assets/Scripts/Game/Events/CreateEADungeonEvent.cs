using System;
using Game.LevelGenerator;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.Quests;

namespace Game.Events
{
    public delegate void CreateEADungeonEvent(object sender, CreateEADungeonEventArgs e);
    public class CreateEADungeonEventArgs : EventArgs
    {
        private Fitness fitness;
        public QuestLine QuestLineForDungeon { get; set; }
        private string playerProfile;

        public CreateEADungeonEventArgs(Fitness fitness)
        {
            Fitness = fitness;
            QuestLineForDungeon = null;
        }
        //TODO refactor Fitness to accept only the parameter classes
        public CreateEADungeonEventArgs(QuestLine questLine)
        {
            QuestLineForDungeon = questLine;
            QuestDungeonsParameters questDungeonParameters = questLine.DungeonParametersForQuestLine;
            QuestEnemiesParameters questEnemiesParameters = questLine.EnemyParametersForQuestLine;
            Fitness = new Fitness(questDungeonParameters.Size, questDungeonParameters.NKeys, 
                questDungeonParameters.NKeys, questEnemiesParameters.NEnemies, questDungeonParameters.GetLinearity());
        }
    
        public Fitness Fitness { get => fitness; set => fitness = value; }
    }
}