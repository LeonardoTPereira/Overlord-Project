using Util;
using System;
using Game.NPCs;
using System.Collections.Generic;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ListenQuestSo : ImmersionQuestSo
    {
        public override string SymbolType => Constants.ListenQuest;

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get => _nextSymbolChances;
            set => _nextSymbolChances = value;
        }

        //No NPCSo directly. It must be only the job/race, defined using some method based on the next quest
        public NpcSo Npc { get; set; }

        public override void Init()
        {
            base.Init();
            Npc = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, NpcSo npc)
        {
            base.Init(questName, endsStoryLine, previous);
            Npc = npc;
        }

        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var listenQuest = copiedQuest as ListenQuestSo;
            if (listenQuest != null)
            {
                Npc = listenQuest.Npc;
            }
            else
            {
                throw new ArgumentException(
                    $"Expected argument of type {typeof(ListenQuestSo)}, got type {copiedQuest.GetType()}");
            }
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ListenQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            return !IsCompleted && Id == questId;
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            IsCompleted = true;
        }

        public override void CreateQuestString()
        {
            QuestText = $"{Npc.NpcName}.\n";
        }
    }
}
