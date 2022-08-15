using System;
using System.Threading.Tasks;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Game.NarrativeGenerator.Quests;

namespace Game.Events
{
    public delegate Task CreateEaDungeonEvent(object sender, CreateEaDungeonEventArgs e);
    public class CreateEaDungeonEventArgs : EventArgs
    {
        public Parameters Parameters { get ; set ; }

        public CreateEaDungeonEventArgs(Parameters parameters)
        {
            Parameters = parameters;
        }
        public CreateEaDungeonEventArgs(QuestLineList questLines)
        {
            var questDungeonParameters = questLines.DungeonParametersForQuestLines;
            var questEnemies = questLines.EnemyParametersForQuestLines.NEnemies;
            var questItems = questLines.ItemParametersForQuestLines.TotalItems;
            var questNpcs = questLines.NpcSos.Count;
            var rooms = questDungeonParameters.Size;
            var keys = questDungeonParameters.NKeys;
            var locks = keys;
            var linearity = questDungeonParameters.GetLinearity();
            var fitnessParameters = new FitnessParameters(rooms, keys, locks, questEnemies, linearity, questItems
                , questNpcs);
            Parameters = new Parameters(fitnessParameters);
        }
    }
}