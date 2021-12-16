using System;

namespace LevelGenerator
{
    /// This struct holds the most relevant data of the evolutionary process.
    [Serializable]
    public struct Data
    {
        public Parameters parameters { get; set; }
        public double duration { get; set; }
    }
}