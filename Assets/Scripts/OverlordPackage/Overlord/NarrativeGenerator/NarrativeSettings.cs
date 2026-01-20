using MyBox;
using Overlord.NarrativeGenerator.NPCs;
using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Overlord.NarrativeGenerator
{
    [CreateAssetMenu(fileName = "NarrativeSettingsSO", menuName = "Overlord-Project/Narrative-Generator/NarrativeSettingsSO")]
    public class NarrativeSettings : ScriptableObject
    {        
        [field: SerializeField] public List<NpcSo> PlaceholderNpcs { get; set; }
        [field: Foldout("Item Sets", true)]
        [field: SerializeField] public TreasureRuntimeSetSo PlaceholderItems { get; set; }
        [field: SerializeField] public TreasureRuntimeSetSo Gemstones { get; set; }
        [field: SerializeField] public TreasureRuntimeSetSo Tools { get; set; }
        [field: SerializeField] public TreasureRuntimeSetSo ReadableItems { get; set; }
        [field: SerializeField] public WeaponTypeRuntimeSetSO PossibleWeapons { get; set; }

        [field: Foldout("Quest Terminal Parameters", true)]
        [field: MinMaxRange(1, 100), SerializeField] public RangedInt EnemiesToKill { get; set; }
        [field: MinMaxRange(0, 100), SerializeField] public RangedInt ItemsToGather { get; set; }
        [field: MinMaxRange(1, 100), SerializeField] public RangedInt RoomsToExplore { get; set; }
    }
}

