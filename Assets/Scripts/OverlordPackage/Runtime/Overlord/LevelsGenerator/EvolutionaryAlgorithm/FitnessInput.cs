using System;
using System.Collections.Generic;
using Overlord.ProfileAnalyst;
using MyBox;
using UnityEngine;
using Overlord.NarrativeGenerator.Quests;
using Overlord.LevelGenerator.Manager;

namespace Overlord.LevelGenerator.EvolutionaryAlgorithm
{
    [Serializable]
    public class FitnessInput
    {
        static public FitnessDesiredValuesSO DesiredValues;
        public IEnumerable<QuestLine> QuestLines { get; private set; }
        public YeePlayerProfile PlayerProfile { get; private set; }

        public FitnessInput(int rooms, int keys, int locks, int enemies, float linearCoefficient, int items, int npcs,
            IEnumerable<QuestLine> questLines, YeePlayerProfile playerProfile)
        {
            DesiredRooms = rooms;
            DesiredKeys = keys;
            DesiredLocks = locks;
            DesiredEnemies = enemies;
            DesiredItems = items;
            DesiredNpcs = npcs;
            DesiredLinearity = linearCoefficient;
            QuestLines = questLines;
            PlayerProfile = playerProfile;
        }
        
        public int DesiredRooms { get => DesiredValues.desiredRooms; set => DesiredValues.desiredRooms = value; }
        public int DesiredKeys { get => DesiredValues.desiredKeys; set => DesiredValues.desiredKeys = value; }
        public int DesiredLocks { get => DesiredValues.desiredLocks; set => DesiredValues.desiredLocks = value; }
        public int DesiredEnemies { get => DesiredValues.desiredEnemies; set => DesiredValues.desiredEnemies = value; }
        public int DesiredItems { get => DesiredValues.desiredItems; set => DesiredValues.desiredItems = value; }
        public int DesiredNpcs { get => DesiredValues.desiredNpcs; set => DesiredValues.desiredNpcs = value; }
        public float DesiredLinearity { get => DesiredValues.desiredLinearity; set => DesiredValues.desiredLinearity = value; }
    }
}