using System;

[Serializable]
public class UnfeasibleLevelException : Exception
{
    public UnfeasibleLevelException() : base($"Dungeon is infeasible") 
    {
        
    }
}