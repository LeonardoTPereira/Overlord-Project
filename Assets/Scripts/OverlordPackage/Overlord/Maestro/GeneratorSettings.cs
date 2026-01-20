using MyBox;
using Overlord.NarrativeGenerator.NPCs;
using ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Game.Maestro.ExperimentControllers
{
    [CreateAssetMenu(fileName = "GeneratorSettings", menuName = "Settings/GeneratorSettings")]
    public class GeneratorSettings : ScriptableObject
    {
        [field: SerializeField] public Enums.GameType GameType { get; set; }

        [field: Foldout("Room Generator Parameters", true)]

        [field: SerializeField] public bool CreateRooms { get; set; }
        [field: SerializeField] public Vector2 RoomSize { get; set; }

        [field: Foldout("Experiment Parameters", true)]
        [field: SerializeField] public bool UseLevelSelect { get; set; }
        [field: SerializeField] public bool GenerateInRealTime { get; set; }
        [field: SerializeField] public bool EnableRandomProfileToPlayer { get; set; }
        [field: SerializeField] public int ProbabilityToGetTrueProfile { get; set; }
        [field: SerializeField] public int TotalRunsOfEA { get; set; }        
    }
}