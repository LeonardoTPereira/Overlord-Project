using Game.ExperimentControllers;
using Overlord.LevelGenerator.EvolutionaryAlgorithm;
using System;

namespace Overlord.LevelGenerator.Manager
{
    public class TopdownLevelGeneratorManager : LevelGeneratorManager
    {
        private void OnEnable()
        {
            DungeonMapEliteVisualizer.ContinueGenerationEventHandler += ContinueGenerationEvent;
        }

        private void ContinueGenerationEvent(object sender, EventArgs e)
        {
            _generator.waitGeneration = false;
        }

        private void OnDisable()
        {
            DungeonMapEliteVisualizer.ContinueGenerationEventHandler -= ContinueGenerationEvent;
        }
    }
}