using System;
using System.Threading.Tasks;
using Game.ExperimentControllers;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Game.NarrativeGenerator.Quests;

namespace Game.Events
{
    public delegate Task CreateEaDungeonEvent(object sender, CreateEaDungeonEventArgs e);
    public class CreateEaDungeonEventArgs : EventArgs
    {
        public GeneratorSettings.Parameters Parameters { get ; set ; }
        public FitnessInput Fitness{ get ; set ; }

        public CreateEaDungeonEventArgs(GeneratorSettings.Parameters parameters, FitnessInput fitness)
        {
            Parameters = parameters;
            Fitness = fitness;
        }
        public CreateEaDungeonEventArgs(QuestLineList questLines, GeneratorSettings.Parameters dungeonParameters)
        {
            var questDungeonParameters = questLines.DungeonParametersForQuestLines;
            var questEnemies = questLines.EnemyParametersForQuestLines.NEnemies;
            var questItems = questLines.ItemParametersForQuestLines.TotalItems;
            var questNpcs = questLines.NpcSos.Count;
            var rooms = questDungeonParameters.Size;
            var keys = questDungeonParameters.NKeys;
            var locks = keys;
            var linearity = questDungeonParameters.GetLinearity();
            Fitness = new FitnessInput(rooms, keys, locks, questEnemies, linearity, questItems, questNpcs);
            Parameters = dungeonParameters;
        }
    }
}