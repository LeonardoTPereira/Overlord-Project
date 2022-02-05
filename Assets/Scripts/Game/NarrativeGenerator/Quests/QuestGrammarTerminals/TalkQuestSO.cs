using ScriptableObjects;
using Util;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    class TalkQuestSO : QuestSO
    {
        public NpcSO npc { get; set; }
        public override string symbolType { 
            get { return Constants.TALK_TERMINAL;} 
        }

        public override void Init()
        {
            base.Init();
            npc = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSO previous, NpcSO npc)
        {
            base.Init(questName, endsStoryLine, previous);
            this.npc = npc;
        }
        
        public void AddNpc(NpcSO npc)
        {
            this.npc = npc;
        }
    }
}
