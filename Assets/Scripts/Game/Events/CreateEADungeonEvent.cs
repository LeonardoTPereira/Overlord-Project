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

        public CreateEADungeonEventArgs(Parameters parameters)
        {
            Parameters = parameters;
        }
        public CreateEADungeonEventArgs(QuestLine questLine)
        {
            var questDungeonParameters = questLine.DungeonParametersForQuestLine;
            var questEnemies = questLine.EnemyParametersForQuestLine.NEnemies;
            var questItems = questLine.ItemParametersForQuestLine.TotalItems;
            var questNpcs = questLine.NpcParametersForQuestLine.TotalNpcs;
            var rooms = questDungeonParameters.Size;
            var keys = questDungeonParameters.NKeys;
            var locks = keys;
            var linearity = questDungeonParameters.GetLinearity();
            var fitnessParameters = new FitnessParameters(rooms, keys, locks, questEnemies, linearity, questItems
                , questNpcs);
            Parameters = new Parameters(fitnessParameters);
        }
    
        public Parameters Parameters { get => parameters; set => parameters = value; }
    }
}