using MyBox;
using ScriptableObjects;
using UnityEngine;
using Overlord.RulesGenerator.EnemyGeneration;
using System;

//namespace Overlord.RulesGenerator.EnemyGeneration { } trazer depois para a pasta correta (Overlord/RulesGenerator)
[System.Serializable]
public class SearchSpaceConfig
{
    [MinMaxRange(.1f, 10f)]
    public RangedFloat Status1 = new RangedFloat(1, 6);
    [MinMaxRange(.1f, 10f)]
    public RangedFloat Status2 = new RangedFloat(1, 4);
    [MinMaxRange(.1f, 10f)]
    public RangedFloat Status3 = new RangedFloat(0.75f, 4f);
    [MinMaxRange(.1f, 10f)]
    public RangedFloat Status4 = new RangedFloat(0.8f, 3.2f);
    [MinMaxRange(.1f, 10f)]
    public RangedFloat Status5 = new RangedFloat(1.5f, 10f);
    [MinMaxRange(.1f, 10f)] 
    public RangedFloat Status6 = new RangedFloat(0.3f, 1.5f);
    [MinMaxRange(.1f, 10f)]
    public RangedFloat WeaponStatus1 = new RangedFloat(1f, 4f);

    //[DisplayInspector]
    //public EnemyWeaponType WeaponType;
    //[field: Foldout("Enemy Components")]
    [DisplayInspector]
    public EnemyMovementsSOInterface MovementSet;
    //[field: Foldout("Enemy Components")]
    public WeaponTypeRuntimeSetSO WeaponSet;
    //[SerializeField] private BehaviorTypeRuntimeSetSO BehaviorSet;
    
    
}
