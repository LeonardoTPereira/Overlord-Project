using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests
{
    class TalkQuestSO : QuestSO
    {
        NpcSO npc;

        public TalkQuestSO ()
        {
            symbolType = SymbolType.talk;
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
