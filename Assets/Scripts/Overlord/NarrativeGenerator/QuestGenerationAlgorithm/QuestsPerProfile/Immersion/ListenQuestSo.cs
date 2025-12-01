using Overlord.NarrativeGenerator.NPCs;
using System;
using System.Collections.Generic;
using System.Text;
using Util;
using static Util.Enums;

namespace Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals
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
        public bool HasCreatedDialogue { get; set; }

        public override void Init()
        {
            base.Init();
            Npc = null;
            HasCreatedDialogue = false;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, NpcSo npc)
        {
            base.Init(questName, endsStoryLine, previous);
            Npc = npc;
            HasCreatedDialogue = false;
        }

        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var listenQuest = copiedQuest as ListenQuestSo;
            if (listenQuest != null)
            {
                Npc = listenQuest.Npc;
                HasCreatedDialogue = listenQuest.HasCreatedDialogue;
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

        public override string GetTargetNpc()
        {
            return Npc.NpcName;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            return !IsCompleted && Id == questId;
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            IsCompleted = true;
        }

        public override void CreateQuestString(Language language)
        {
            var stringBuilder = new StringBuilder();

            if (language == Language.Portuguese)
            {
                QuestText = $"Fale com {Npc.NpcName}.\n";
                return;
            }
            QuestText = $"Listen to {Npc.NpcName}.\n";
        }
    }
}