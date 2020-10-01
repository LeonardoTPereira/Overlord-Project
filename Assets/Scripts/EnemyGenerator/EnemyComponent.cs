using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace EnemyGenerator
{
#if UNITY_EDITOR
    //The enemy component with all its attributes
    public struct EnemyComponent : IComponentData
    {

        public int health;
        public int damage;
        public float movementSpeed;
        public float activeTime;
        public float restTime;
        //public WeaponEnum weapon;
        //public MovementEnum movement;
        public int weapon;
        public int movement;
        public int behavior;
        public float fitness;

    }

    //Differentiates population from the intermediate one
    public struct Population : IComponentData { }
    public struct IntermediatePopulation : IComponentData
    {
        public int parent1;
        public int parent2;
    }
    public struct ElitePopulation : IComponentData { }

    /*public enum WeaponEnum
    {
        None,
        Sword,
        Shield,
        Bow,
        Bomb,
        COUNT
    }*/
#endif

    public enum MovementEnum
    {
        None,
        Random,
        Follow,
        Flee,
        Random1D,
        Follow1D,
        Flee1D,
        COUNT
    }

}