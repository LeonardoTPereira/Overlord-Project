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
            Dictionary<string, float> questWeightsbyType = new Dictionary<string, float>
            {
                {profileDropdowns["Dropdown1"].captionText.text, (float) QuestWeights.Loved},
                {profileDropdowns["Dropdown2"].captionText.text, (float) QuestWeights.Liked},
                {profileDropdowns["Dropdown3"].captionText.text, (float) QuestWeights.Disliked},
                {profileDropdowns["Dropdown4"].captionText.text, (float) QuestWeights.Hated}
            };
            NarrativeCreatorEventHandler?.Invoke(this, new NarrativeCreatorEventArgs(questWeightsbyType));
        }
    }
}
