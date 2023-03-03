using System;
using System.Collections.Generic;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Game.LevelManager;
using Game.LevelManager.DungeonLoader;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.LevelGenerator.LevelSOs
{
    [Serializable, CreateAssetMenu]
    public class DungeonFileSo : ScriptableObject, ISavableGeneratedContent
    {
        const string Foldername = "Assets/Resources/Experiment/Dungeons";
        [field: SerializeField] public Dimensions DungeonSizes { get; set; }
        [field: SerializeField] public List<DungeonRoomData> Parts { get; set; }
        [field: SerializeField] public Fitness FitnessFromEa { get; set; }
        public float ExplorationCoefficient { get; set; }
        public float LeniencyCoefficient { get; set; }
        public string BiomeName { get; set; }
        public int TotalEnemies { get; set; }
        public int TotalTreasures { get; set; }
        public int TotalNpcs { get; set; }
        private int _currentIndex = 0;

        public void ResetIndex()
        {
            _currentIndex = 0;
        }

        public DungeonPart GetNextPart(Enums.GameType gameType)
        {

            if (_currentIndex < Parts.Count)
                return DungeonPartFactory.CreateDungeonPartFromDungeonFileSO(Parts[_currentIndex++], gameType);

            return null;
        }

        public void Init(Dimensions dimensions, List<DungeonRoomData> rooms, Fitness fitness, float exploration, float leniency, string biome)
        {
            DungeonSizes = dimensions;
            Parts = rooms;
            FitnessFromEa = fitness;
            ExplorationCoefficient = exploration;
            LeniencyCoefficient = leniency;
        }
        
        public void SaveAsset(string directory)
        {
#if UNITY_EDITOR
            const string newFolder = "Dungeons";
            var fileName = directory;
            if (!AssetDatabase.IsValidFolder(fileName + Constants.SeparatorCharacter + newFolder))
            {
                AssetDatabase.CreateFolder(fileName, newFolder);
            }
            fileName += Constants.SeparatorCharacter + newFolder;
            fileName += Constants.SeparatorCharacter;
            fileName += GetFilename()+".asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
#endif
        }
        
        private string GetFilename()
        {

            // Set the dungeon filename
            string filename = "";
            filename = "R" + FitnessFromEa.DesiredInput.DesiredRooms +
                       "-K" + FitnessFromEa.DesiredInput.DesiredKeys +
                       "-L" + FitnessFromEa.DesiredInput.DesiredLocks +
                       "-E" + FitnessFromEa.DesiredInput.DesiredEnemies +
                       "-L" + FitnessFromEa.DesiredInput.DesiredLinearity;
            return filename;
        }
    }
}