using System;
using System.Threading.Tasks;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Game.NarrativeGenerator.Quests;

namespace Game.Events
{
    public delegate Task CreateEADungeonEvent(object sender, CreateEADungeonEventArgs e);
    public class CreateEADungeonEventArgs : EventArgs
    {
        private Parameters parameters;
        public QuestLine QuestLineForDungeon { get; set; }
        private string playerProfile;

        public CreateEADungeonEventArgs(Parameters parameters)
        {
            Parameters = parameters;
            QuestLineForDungeon = null;
        }
        public CreateEADungeonEventArgs(QuestLine questLine)
        {
            QuestLineForDungeon = questLine;
            var questDungeonParameters = questLine.DungeonParametersForQuestLine;
            var questEnemiesParameters = questLine.EnemyParametersForQuestLine;
            var fitnessParameters = new FitnessParameters(questDungeonParameters.Size,
                questDungeonParameters.NKeys,
                questDungeonParameters.NKeys, questEnemiesParameters.NEnemies, questDungeonParameters.GetLinearity());
            Parameters = new Parameters(fitnessParameters);
        }
    
        public Parameters Parameters { get => parameters; set => parameters = value; }
    }
}