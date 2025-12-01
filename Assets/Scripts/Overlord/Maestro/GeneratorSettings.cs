using MyBox;
using Overlord.NarrativeGenerator.NPCs;
using ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Overlord.Maestro.ExperimentControllers
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

        [field: Foldout("EA Parameters", true)]
        [field: SerializeField] public Parameters DungeonParameters { get; set; }

        [Serializable]
        public struct Parameters
        {
            [field: SerializeField, Range(1, 240)] public int Time { get; set; }
            [field: SerializeField, Range(1, 100)] public int Population { get; set; }
            [field: SerializeField, Range(0, 100)] public int Mutation { get; set; }
            [field: SerializeField, Range(1, 10)] public int Competitors { get; set; }
            [field: SerializeField, Range(1, 200)] public int MinimumElite { get; set; }
            [field: SerializeField, Range(0.0f, 10.0f)] public float AcceptableFitness { get; set; }
        }
    }
}