﻿using UnityEngine;
using Util;

public delegate Vector3 MovementType(Vector3 playerPos, Vector3 enemyPos);

[CreateAssetMenu]
public class MovementTypeSO : ScriptableObject
{

    public float multiplier;
    public Enums.MovementEnum enemyMovementIndex;
    public MovementType movementType;
}