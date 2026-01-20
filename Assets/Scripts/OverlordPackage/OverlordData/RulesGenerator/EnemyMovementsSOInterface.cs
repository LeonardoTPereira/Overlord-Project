using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovementsSOInterface : ScriptableObject
{
    public abstract int GetMappedIndex(Enum movement);
    public abstract string GetMovementName(int index);
    public abstract Enum GetEnemyMovementByIndex(int index);
    public abstract int GetEnemyMovementCount();
    public abstract List<Enum> GetAllMovementTypes();
    public abstract List<Enum> GetAllMovementEnums();
    // TODO: Remove this method after separating scripts that deal with specific enemies from Topdown
    public abstract List<Enum> GetHealerMovementList();
}
