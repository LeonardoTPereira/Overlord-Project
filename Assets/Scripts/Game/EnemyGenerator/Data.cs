using System;
using System.Collections.Generic;

namespace Game.EnemyGenerator
{
    /// This struct holds the most relevant data of the evolutionary process.
    [Serializable]
    public struct Data
    {
        public Parameters parameters { get; set; }
        public double duration { get; set; }
        public List<Individual> initial { get; set; }
        public List<Individual> intermediate { get; set; }
        public List<Individual> final { get; set; }
    }
}