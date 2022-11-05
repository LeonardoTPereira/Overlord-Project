using System.Collections.Generic;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.ExperimentControllers
{
    [CreateAssetMenu(fileName = "GeneratorSettings", menuName = "Settings/GeneratorSettings")]
    public class GeneratorSettings : ScriptableObject
    {
        [field: SerializeField] public Enums.GameType GameType { get; set; }
        [field: SerializeField] public bool CreateRooms { get; set; }
        [field: SerializeField] public bool UseLevelSelect { get; set; }
        [field: SerializeField] public bool GenerateInRealTime { get; set; }
        [field: SerializeField] public bool EnableRandomProfileToPlayer { get; set; }
        [field: SerializeField] public int ProbabilityToGetTrueProfile { get; set; }
        [field: SerializeField] public List<NpcSo> PlaceholderNpcs { get; set; }
        [field: SerializeField] public TreasureRuntimeSetSo PlaceholderItems { get; set; }
        [field: SerializeField] public TreasureRuntimeSetSo Gemstones { get; set; }
        [field: SerializeField] public TreasureRuntimeSetSo Tools { get; set; }
        [field: SerializeField] public WeaponTypeRuntimeSetSO PossibleWeapons { get; set; }
        
        [field: SerializeField] public Vector2 RoomSize { get; set; }
    }
}