using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.NPCs
{
    [CreateAssetMenu(fileName = "NpcSO", menuName = "NpcModule/Npcs", order = 0)]
    [Serializable]
    public class NpcSo : ScriptableObject
    {
        
        [FormerlySerializedAs("_npcName")] [SerializeField] private string npcName;
        [SerializeField] private Jobs job;
        [SerializeField] private Races race;
        [SerializeField] private int violenceFactor;
        [SerializeField] private int attainFactor;
        [SerializeField] private int explorationFactor;
        [SerializeField] private int socialFactor;
        [SerializeField] private Sprite gameSprite;
        [SerializeField] private NpcDialogueData dialogueData;

        public string NpcName
        {
            get => npcName;
            set => npcName = dialogueData.NpcDialogueName = value;
        }

        public Jobs Job
        {
            get => job;
            set => job = value;
        }

        public Races Race
        {
            get => race;
            set => race = value;
        }

        public int ViolenceFactor
        {
            get => violenceFactor;
            set => violenceFactor = value;
        }

        public int AttainFactor
        {
            get => attainFactor;
            set => attainFactor = value;
        }

        public int ExplorationFactor
        {
            get => explorationFactor;
            set => explorationFactor = value;
        }

        public int SocialFactor
        {
            get => socialFactor;
            set => socialFactor = value;
        }
        
        public Sprite GameSprite
        {
            get => gameSprite;
            set => gameSprite = value;
        }

        public NpcDialogueData DialogueData
        {
            get => dialogueData;
            set => dialogueData = value;
        }
    }
}
