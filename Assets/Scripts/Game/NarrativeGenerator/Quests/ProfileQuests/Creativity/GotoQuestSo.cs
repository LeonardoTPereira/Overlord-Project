using Util;
using System;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GotoQuestSo : CreativityQuestSo
    {
        public override string SymbolType => Constants.GOTO_QUEST;
        public Coordinates SelectedRoomCoordinates { get; set; }

        public override void Init()
        {
            base.Init();
            SelectedRoomCoordinates = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, Coordinates selectedRoomCoordinates)
        {
            base.Init(questName, endsStoryLine, previous);
            SelectedRoomCoordinates = selectedRoomCoordinates;
        }

        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var goToQuestSo = copiedQuest as GotoQuestSo;
            if (goToQuestSo != null)
            {
                SelectedRoomCoordinates = goToQuestSo.SelectedRoomCoordinates;
            }
            else
            {
                throw new ArgumentException(
                    $"Expected argument of type {typeof(GotoQuestSo)}, got type {copiedQuest.GetType()}");
            }
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<GotoQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            return !IsCompleted 
                   && (questElement as Coordinates 
                       ?? throw new InvalidOperationException()).Equals(SelectedRoomCoordinates);
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            IsCompleted = true;
        }

        //TODO highlight the room in the Map UI
        public override string ToString()
        {
            return "Go to the room highlighted in the map";
        }
    }
}