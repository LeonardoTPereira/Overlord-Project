using System.Collections.Generic;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;

namespace Game.ExperimentControllers
{
    [CreateAssetMenu(fileName = "GeneratorSettings", menuName = "Settings/GeneratorSettings")]
    public class GeneratorSettings : ScriptableObject
    {
        [field: SerializeField] public bool CreateRooms { get; set; }
        [field: SerializeField] public bool UseLevelSelect { get; set; }
        [field: SerializeField] public bool GenerateInRealTime { get; set; }
        [field: SerializeField] public bool EnableRandomProfileToPlayer { get; set; }
        [field: SerializeField] public int ProbabilityToGetTrueProfile { get; set; }
        [field: SerializeField] public List<NpcSo> PlaceholderNpcs { get; set; }
        [field: SerializeField] public TreasureRuntimeSetSO PlaceholderItems { get; set; }
        [field: SerializeField] public WeaponTypeRuntimeSetSO PossibleWeapons { get; set; }
    }
}