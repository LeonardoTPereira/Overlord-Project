using System.Collections.Generic;
using System.Linq;
using Game.Events;
using TMPro;
using UnityEngine;
using static Util.Enums;

namespace Game.NarrativeGenerator
{
    public class NarrativeGenerator : MonoBehaviour
    {
        [SerializeField] protected GameObject dropdownCanvas;
        protected Dictionary<string, TMP_Dropdown> profileDropdowns;
        public static event NarrativeCreatorEvent NarrativeCreatorEventHandler;

        public void Awake()
        {
            profileDropdowns = dropdownCanvas.GetComponentsInChildren<TMP_Dropdown>()
                .ToDictionary(key => key.name, inputFieldObj => inputFieldObj);
        }

        public void SelectQuestWeights()
        {
            Dictionary<string, int> questWeightsbyType = new Dictionary<string, int>
            {
                {profileDropdowns["Dropdown1"].captionText.text, (int) QuestWeights.Loved},
                {profileDropdowns["Dropdown2"].captionText.text, (int) QuestWeights.Liked},
                {profileDropdowns["Dropdown3"].captionText.text, (int) QuestWeights.Disliked},
                {profileDropdowns["Dropdown4"].captionText.text, (int) QuestWeights.Hated}
            };
            NarrativeCreatorEventHandler?.Invoke(this, new NarrativeCreatorEventArgs(questWeightsbyType));
        }
    }
}
