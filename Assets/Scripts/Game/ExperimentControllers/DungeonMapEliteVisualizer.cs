using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Game.Events;
using Game.LevelGenerator;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Game.LevelGenerator.LevelSOs;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Game.ExperimentControllers
{
    public class DungeonMapEliteVisualizer : MonoBehaviour
    {
        private LevelGeneratorManager _levelGeneratorManager;
        private DungeonSOTester _dungeonSoTester;
        private List<DungeonFileSo> _generatedDungeons;
        private int _currentDungeon;
        private int _maxEnemies;
        [SerializeField] private Camera textureCamera;
        [field: SerializeField] private FitnessInput Fitness { get; set; }
        public static EventHandler ContinueGenerationEventHandler;
        public GeneratorSettings generatorSettings;

        private void Awake()
        {
            _currentDungeon = 0;
        }

        private void Start()
        {
            _levelGeneratorManager = GetComponent<LevelGeneratorManager>();
            _dungeonSoTester = GetComponent<DungeonSOTester>();
        }

        private void OnEnable()
        {
            ClassicEvolutionaryAlgorithm.CurrentGenerationEventHandler += PrintCurrentPopulation;
            LevelGenerator.LevelGenerator.CurrentGenerationEventHandler += PrintCurrentPopulation;
        }

        private void PrintCurrentPopulation(object sender, CurrentGenerationEventArgs e)
        {
            _generatedDungeons = new List<DungeonFileSo>();
            foreach (var individual in e.CurrentPopulation.EliteList)
            {
                var dungeon =
                    Interface.CreateDungeonSoFromIndividual(individual, Fitness.DesiredEnemies, Fitness.DesiredItems, Fitness.DesiredNpcs);
                _generatedDungeons.Add(dungeon);
            }
            Debug.Log("Finished");
            _currentDungeon = 0;
            _maxEnemies = GetMaxEnemies(_generatedDungeons);
            var center = GetDungeonCenter(_generatedDungeons[_currentDungeon]);
            _dungeonSoTester.DrawDungeonSprites(_generatedDungeons[_currentDungeon++], _maxEnemies, center);
        }

        private void OnDisable()
        {
            ClassicEvolutionaryAlgorithm.CurrentGenerationEventHandler -= PrintCurrentPopulation;
            LevelGenerator.LevelGenerator.CurrentGenerationEventHandler -= PrintCurrentPopulation;
        }

        public async void Create(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                await CreateDungeonsForQuestLine();
            }
        }

        public void Continue(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ContinueGenerationEventHandler?.Invoke(this, EventArgs.Empty);
            }
        }
        
        public void PrintScreen(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            if (context.performed)
            {
                PrintCurrentDungeon();
            }
#endif
        }
        
        public void VisualizeNext(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            var center = GetDungeonCenter(_generatedDungeons[_currentDungeon]);
            _dungeonSoTester.DrawDungeonSprites(_generatedDungeons[_currentDungeon++], _maxEnemies, center);
            _currentDungeon %= _generatedDungeons.Count;
        }
        
        private async Task CreateDungeonsForQuestLine()
        {
            _generatedDungeons = await _levelGeneratorManager.EvolveDungeonPopulation(new CreateEaDungeonEventArgs(
                generatorSettings.DungeonParameters, Fitness, true));
            Debug.Log("Finished");
            _maxEnemies = GetMaxEnemies(_generatedDungeons);
            var center = GetDungeonCenter(_generatedDungeons[_currentDungeon]);
            _dungeonSoTester.DrawDungeonSprites(_generatedDungeons[_currentDungeon++], _maxEnemies, center);
            
        }

        private static Vector3 GetDungeonCenter(DungeonFileSo generatedDungeon)
        {
            var min = new Coordinates(0, 0);
            var max = new Coordinates(0, 0);
            foreach (var room in generatedDungeon.Parts)
            {
                var x = room.Coordinates.X;
                var y = room.Coordinates.Y;
                if (x < min.X)
                {
                    min.X = x;
                }

                if (x > max.X)
                {
                    max.X = x;
                }

                if (y < min.Y)
                {
                    min.Y = y;
                }

                if (y > max.Y)
                {
                    max.Y = y;
                }
            }
            return new Vector3((max.X + min.X)/2f, (max.Y + min.Y)/2f, -15);
        }

        private static int GetMaxEnemies(IEnumerable<DungeonFileSo> generatedDungeons)
        {
            return (from dungeon in generatedDungeons from room in dungeon.Parts select room.TotalEnemies).Prepend(0).Max();
        }
#if UNITY_EDITOR
        public void PrintCurrentDungeon()
        {
            var activeRenderTexture = RenderTexture.active;
            RenderTexture.active = textureCamera.targetTexture;
 
            textureCamera.Render();

            var targetTexture = textureCamera.targetTexture;
            var image = new Texture2D(targetTexture.width, targetTexture.height);
            image.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = activeRenderTexture;
 
            var bytes = image.EncodeToPNG();
            Destroy(image);
            var assetPath = GetDungeonPrintAssetPath(_generatedDungeons[_currentDungeon]);
            File.WriteAllBytes(assetPath + ".png", bytes);
        }

        private string GetDungeonPrintAssetPath(DungeonFileSo dungeon)
        {
            
            var parentDirectory = "Assets"+ Constants.SeparatorCharacter + "Resources" + Constants.SeparatorCharacter + "DungeonPrints";
            var directoryPath = "R_"+dungeon.FitnessFromEa.DesiredInput.DesiredRooms
                                      +"_K_" +dungeon.FitnessFromEa.DesiredInput.DesiredKeys
                                      +"_L_" +dungeon.FitnessFromEa.DesiredInput.DesiredLocks
                                      +"_Lin_" +dungeon.FitnessFromEa.DesiredInput.DesiredLinearity
                                      +"_E_" +dungeon.FitnessFromEa.DesiredInput.DesiredEnemies;
            if (!AssetDatabase.IsValidFolder(parentDirectory + Constants.SeparatorCharacter + directoryPath))
            {
                AssetDatabase.CreateFolder(parentDirectory, directoryPath);
            }
            Debug.Log("Leniency: " + dungeon.LeniencyCoefficient);
            Debug.Log("Exploration: " + dungeon.ExplorationCoefficient);
            Debug.Log("Enemies: " + dungeon.TotalEnemies);
            var leniencyBin = (int)(dungeon.LeniencyCoefficient * 10.00001f) % 6;
            var explorationBin = (int)(dungeon.ExplorationCoefficient * 10.00001f) % 5 + 1;
            var assetPath = parentDirectory + Constants.SeparatorCharacter + directoryPath +
                            Constants.SeparatorCharacter
                            + "L." + leniencyBin + "E." + explorationBin;
            return assetPath;
        }
#endif
    }
}