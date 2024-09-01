using System;
using System.Collections.Generic;
using Game.LevelGenerator.EvolutionaryAlgorithm;

namespace Game.Events
{
    public delegate void CurrentGenerationEvent(object sender, CurrentGenerationEventArgs e);

    public class CurrentGenerationEventArgs : EventArgs
    {
        public Population CurrentPopulation { get; set; }
        public CurrentGenerationEventArgs(Population pop)
        {
            CurrentPopulation = pop;
        }
    }
}