﻿using Game.Events;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public class ClassicPopulation : Population
    {
        public ClassicPopulation(int explorationSize, int leniencySize, int linearitySize, FitnessPlot plotter = null) : base(explorationSize, leniencySize, linearitySize, plotter)
        {
        }
        
        public void UpdateFitnessPlot(int generation)
        {
            if (Plotter == null) return;
            Plotter.UpdateFitnessPlotClassicData(EliteList[0] , generation);
            UnityMainThreadDispatcher.Instance().Enqueue(Plotter.AddAnimationCurves);
        }
    }
}