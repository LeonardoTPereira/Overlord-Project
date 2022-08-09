using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using Game.NPCs;
using MyBox;
using ScriptableObjects;
using Util;

namespace Game.NarrativeGenerator
{
    public class Selector
    {
        Dictionary<string,bool> wasQuestAdded = new Dictionary<string,bool>();

        public void CreateMissions(QuestGeneratorManager m)
        {
            m.Quests.questLines = DrawMissions(m.PlaceholderNpcs, m.PlaceholderItems, m.PossibleWeapons);
        }

        private List<QuestList> DrawMissions(List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            CreateQuestDict();
            var questLineList = new List<QuestList>();
            do
            {
                var questLine = new QuestList();
                MarkovChain questChain = new MarkovChain();
                while ( questChain.GetLastSymbol().CanDrawNext )
                {
                    ISymbol lastSelectedQuest = questChain.GetLastSymbol();
                    lastSelectedQuest.SetDictionary( ProfileCalculator.StartSymbolWeights );
                    lastSelectedQuest.SetNextSymbol( questChain );

                    ISymbol nonTerminalSymbol = questChain.GetLastSymbol();
                    UpdateListContents( nonTerminalSymbol );
                    nonTerminalSymbol.SetNextSymbol( questChain );

                    questChain.GetLastSymbol().DefineQuestSo( questLine.Quests, possibleNpcs, possibleTreasures, possibleEnemyTypes );
                }
                questLine.Quests[^1].EndsStoryLine = true;
                questLine.NpcInCharge = possibleNpcs.GetRandom();
                questLineList.Add(questLine);
            //TODO: Verify with Leo if it would be interesting 
            //to have a minumum number of questlines
            } while ( wasQuestAdded.ContainsValue(false) );
            return questLineList;
        }

        private void CreateQuestDict ()
        {
            wasQuestAdded.Add(Constants.ImmersionQuest, false);
            wasQuestAdded.Add(Constants.AchievementQuest, false);
            wasQuestAdded.Add(Constants.MasteryQuest, false);
            wasQuestAdded.Add(Constants.CreativityQuest, false);
        }

        private void UpdateListContents ( ISymbol lastQuest )
        {
            wasQuestAdded[lastQuest.SymbolType] = true;
        }
    }
}