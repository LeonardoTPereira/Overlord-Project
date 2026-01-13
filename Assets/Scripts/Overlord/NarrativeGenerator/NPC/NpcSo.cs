using Overlord.NarrativeGenerator.Quests;
using ScriptableObjects;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Overlord.NarrativeGenerator.NPCs
{
    [CreateAssetMenu(fileName = "NpcSO", menuName = "Overlord-Project/Narrative-Generator/NpcModule/Npcs", order = 0)]
    [Serializable]
    public class NpcSo : ScriptableObject, IDialogueObjSo
    {
        [FormerlySerializedAs("_npcName")] [SerializeField] private string npcName;
        [SerializeField] private int age;
        [SerializeField] private Jobs job;
        [SerializeField] private Races race;
        [SerializeField] private int traumaFactor;
        [SerializeField] private int violenceFactor;
        [SerializeField] private int attainFactor;
        [SerializeField] private int explorationFactor;
        [SerializeField] private int socialFactor;
        [SerializeField] private Sprite gameSprite;
        [SerializeField] private NpcDialogueData dialogueData;
        [SerializeField] private GameObject prefab;
        private Coordinates roomCoordinates;    // Coordinates of the room where the NPC is located. (used only in the spaceshooter game [yet?])

        public string NpcName
        {
            get => npcName;
            set => npcName = dialogueData.NpcDialogueName = value;
        }
        public int Age
        {
            get => age;
            set => age = value;
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

        public int TraumaFactor
        {
            get => traumaFactor;
            set => traumaFactor = value;
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

        public GameObject Prefab
        {
            get => prefab;
        }

        public Coordinates RoomCoordinates
        {
            get => roomCoordinates;
            set => roomCoordinates = value;
        }
    }
}
