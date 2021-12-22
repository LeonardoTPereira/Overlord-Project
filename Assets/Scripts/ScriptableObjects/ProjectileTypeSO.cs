﻿using UnityEngine;
using Util;

namespace ScriptableObjects
{
    public delegate void ProjectileBehavior(Vector2 shootDirection);

    [CreateAssetMenu]
    public class ProjectileTypeSO : ScriptableObject
    {
        public int multiplier;
        public GameObject projectilePrefab;
        //These data are for player's projectiles only (for now)
        public int damage;
        public float atkSpeed, moveSpeed;
        public Enums.PlayerProjectileEnum projectileBehaviorIndex;
        public string projectileName, description;
        public Color color;
    }
}