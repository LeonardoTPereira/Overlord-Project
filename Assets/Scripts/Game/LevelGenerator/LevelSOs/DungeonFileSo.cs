using System;
using System.Collections.Generic;
using System.IO;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Game.LevelManager;
using Game.LevelManager.DungeonLoader;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.LevelGenerator.LevelSOs
{
    [Serializable, CreateAssetMenu]
    public class DungeonFileSo : ScriptableObject, SaveableGeneratedContent
    {
        const string Foldername = "Assets/Resources/Experiment/Dungeons";
        
        [field: SerializeField] public Dimensions DungeonSizes { get; set; }

        [field: SerializeField] public List<SORoom> Rooms { get; set; }
        [field: SerializeField] public Fitness FitnessFromEa { get; set; }
        public float ExplorationCoefficient { get; set; }
        public float LeniencyCoefficient { get; set; }

        private int _currentIndex = 0;

        public void ResetIndex()
        {
            _currentIndex = 0;
        }

        public DungeonPart GetNextPart()
        {
            if (_currentIndex < Rooms.Count)
                return DungeonPartFactory.CreateDungeonPartFromDungeonFileSO(Rooms[_currentIndex++]);
            return null;
        }

        public void Init(Dimensions dimensions, List<SORoom> rooms, Fitness fitness, float exploration, float leniency)
        {
            DungeonSizes = dimensions;
            Rooms = rooms;
            FitnessFromEa = fitness;
            ExplorationCoefficient = exploration;
            LeniencyCoefficient = leniency;
        }
        
        public void SaveAsset(string directory)
        {
#if UNITY_EDITOR
            const string newFolder = "Dungeons";
            var fileName = directory;
            if (!AssetDatabase.IsValidFolder(fileName + Constants.SEPARATOR_CHARACTER + newFolder))
            {
                AssetDatabase.CreateFolder(fileName, newFolder);
            }
            fileName += Constants.SEPARATOR_CHARACTER + newFolder;
            fileName += Constants.SEPARATOR_CHARACTER;
            fileName += GetFilename()+".asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
#endif
        }
        
        private string GetFilename()
        {

            // Set the dungeon filename
            string filename = "";
            filename = "R" + FitnessFromEa.DesiredParameters.DesiredRooms +
                       "-K" + FitnessFromEa.DesiredParameters.DesiredKeys +
                       "-L" + FitnessFromEa.DesiredParameters.DesiredLocks +
                       "-E" + FitnessFromEa.DesiredParameters.DesiredEnemies +
                       "-L" + FitnessFromEa.DesiredParameters.DesiredLinearity;
            return filename;
        }
    }
}