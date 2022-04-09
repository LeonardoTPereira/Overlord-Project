using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSelection
{
    [CreateAssetMenu(fileName = "SelectedLevels", menuName = "Overlord-Project/SelectedLevels", order = 0)]
    [Serializable]
    public class SelectedLevels : ScriptableObject
    {
        [field: SerializeField] public List<LevelData> Levels { get; set; }
    }
}