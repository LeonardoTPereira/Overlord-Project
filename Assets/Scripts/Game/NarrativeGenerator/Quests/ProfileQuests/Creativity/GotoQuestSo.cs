using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GotoQuestSo : CreativityQuestSo
    {
        public override string symbolType {
            get { return Constants.GOTO_QUEST; }
        }
        public int SelectedRoomId { get; set; }

        public override void Init()
        {
            base.Init();
            SelectedRoomId = -1;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, int selectedRoomId)
        {
            base.Init(questName, endsStoryLine, previous);
            SelectedRoomId = selectedRoomId;
        }
        
        public void ChangeRoom(int selectedRoomId)
        {
            SelectedRoomId = selectedRoomId;
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            SelectedRoomId = (copiedQuest as GotoQuestSo).SelectedRoomId;
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<GotoQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }
    }
}