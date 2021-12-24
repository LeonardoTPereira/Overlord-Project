using System;

namespace Game.LevelGenerator
{
    [Serializable]
    public class UnfeasibleLevelException : Exception
    {
        public UnfeasibleLevelException() : base($"Dungeon is infeasible") 
        {
        
        }
    }
}