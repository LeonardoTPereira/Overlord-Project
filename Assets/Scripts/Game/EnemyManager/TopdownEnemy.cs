using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overlord.RulesGenerator;
using ScriptableObjects;
using Util;

public class TopdownEnemy : IEnemy
{
    public int health;
    public int damage;
    public float movementSpeed;
    public float activeTime;
    public float restTime;
    [SerializeField]
    //public WeaponTypeSo weapon;
    //[SerializeField]
    //public MovementTypeSO movement;
    //[SerializeField]
    //public BehaviorTypeSO behavior;
    public float fitness;
    public float attackSpeed;
    public float projectileSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public EnemySO GetEnemySO()
    {
        var enemySO = new EnemySO();
        return enemySO;
    }
    
    
}
