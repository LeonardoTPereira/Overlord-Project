using UnityEngine;
using Game.NarrativeGenerator.Quests;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

namespace Game.NarrativeGenerator
{
    public class QuestUI : MonoBehaviour 
    {
        [SerializeField] private TextMeshProUGUI questList;
        public void CreateQuestList ( List<Narrative> narratives )
        {
            string questDescription = "";
            foreach ( Narrative narrative in narratives )
            {
                string nextQuest = narrative.quests[0].symbolType.ToString();
                if ( narrative.quests[0].symbolType != SymbolType.empty )
                    questDescription += $"{nextQuest}\n";
            }
            questList.text = questDescription;
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