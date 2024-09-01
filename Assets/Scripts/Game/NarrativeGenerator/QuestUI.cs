using UnityEngine;
using Game.NarrativeGenerator.Quests;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Util;

namespace Game.NarrativeGenerator
{
    public class QuestUI : MonoBehaviour 
    {
        [SerializeField] private TextMeshProUGUI questList;
        public void CreateQuestList ( QuestLineList narratives )
        {
            // TODO: Rever este código
            // string questDescription = "";
            // foreach ( QuestLine narrative in narratives.QuestLines )
            // {
            //     string nextQuest = narrative.graph[0].symbolType.ToString();
            //     if ( narrative.graph[0].symbolType != Constants.EMPTY_QUEST )
            //         questDescription += $" - {nextQuest}\n";
            // }
            // questList.text = questDescription;
        }

        public void CreateOpenQuestList ( QuestLine quests )
        {
            // calls create quest list
        }

        public void CreateClosedQuestList ( QuestLine quests )
        {
            // calls create quest list
        }
    }
}