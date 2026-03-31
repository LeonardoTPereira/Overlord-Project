using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlord.LevelGenerator.Manager
{
    [CreateAssetMenu(fileName = "LevelDesiredValuesSO", menuName = "Overlord-Project/Levels-Generator/LevelDesiredValuesSO")]
    public class FitnessDesiredValuesSO : ScriptableObject
    {
        [Foldout("Desired Fitness Values", true)]
        [SerializeField, Range(2, 200)] public int desiredRooms = 20;
        [SerializeField, Range(0, 50)] public int desiredKeys = 4;
        [SerializeField, Range(0, 50)] public int desiredLocks = 4;
        [SerializeField, Range(0, 200)] public int desiredEnemies = 40;
        [SerializeField, Range(0, 200)] public int desiredItems = 10;
        [SerializeField, Range(0, 200)] public int desiredNpcs = 3;
        [SerializeField, Range(1.0f, 3.0f)] public float desiredLinearity = 1.5f;
    }
}