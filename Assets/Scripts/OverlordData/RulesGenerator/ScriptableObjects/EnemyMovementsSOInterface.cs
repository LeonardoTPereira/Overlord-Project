using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovementsSOInterface : ScriptableObject
{
    // Retorna a lista de movimentos (já convertida para Enum genérico)
    public abstract string GetMovementName(int index);
    public abstract Enum GetEnemyMovementByIndex(int index);
    public abstract int GetEnemyMovementCount();
    public abstract List<Enum> GetAllMovementTypes();
    public abstract List<Enum> GetHealerMovementList();
}
