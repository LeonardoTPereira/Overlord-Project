using Game.NPCs;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    class TalkQuestSO : QuestSO
    {
        //No NPCSo directly. It must be only the job/race, defined using some method based on the next quest
        public NpcSo Npc { get; set; }

        public override void Init()
        {
            base.Init();
            Npc = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSO previous, NpcSo npc)
        {
            base.Init(questName, endsStoryLine, previous);
            Npc = npc;
        }
        
        public void AddNpc(NpcSo npc)
        {
            Npc = npc;
        }
    }
}
