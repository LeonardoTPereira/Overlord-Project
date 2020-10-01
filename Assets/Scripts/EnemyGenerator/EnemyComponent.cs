using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace EnemyGenerator
{
    //The enemy component with all its attributes
    public struct EnemyComponent : IComponentData
    {

        public int health;
        public int damage;
        public float movementSpeed;
        public float activeTime;
        public float restTime;
        public WeaponEnum weapon;
        public MovementEnum movement;
        public float fitness;

        public enum WeaponEnum
        {
            None,
            Sword,
            Shield,
            Bow,
            Bomb,
            COUNT
        }

        public enum MovementEnum
        {
            None,
            Random,
            Follow,
            Flee,
            COUNT
        }
    }

    //Differentiates population from the intermediate one
    public struct Population : IComponentData { }
    public struct IntermediatePopulation : IComponentData { }
}