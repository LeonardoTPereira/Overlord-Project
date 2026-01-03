using Overlord.NarrativeGenerator;
using Overlord.RulesGenerator.EnemyGeneration;
using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Util;
using static Util.Enums;

public class SpaceShooterEnemyLoader : MonoBehaviour
{
    public bool useQuestGenerator;
    public EnemyGeneratorManager enemyGenerator;
    public QuestGeneratorManager questGenerator;

    public GameObject enemyPrefab;

    private List<EnemySO> enemiesToSpawn;

    void Start()
    {
        enemiesToSpawn = useQuestGenerator
            ? /*questGenerator.GetEnemySOList()*/ null
            : enemyGenerator.GetEnemySOList(DifficultyLevels.Hard);

        SpawnAll();
    }

    void SpawnAll()
    {
        foreach (var enemySO in enemiesToSpawn)
        {
            SpawnEnemy(enemySO);
        }
    }

    void SpawnEnemy(EnemySO so)
    {
        var enemyGO = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        var enemy = enemyGO.GetComponent<SpaceShooterEnemy>();

        var movement = CreateMovement(enemyGO, so.movement);
        var weapon = CreateWeapon(enemyGO, so.weapon);

        enemy.Init(so, movement, weapon);
    }


    SpaceShooterMovement CreateMovement(GameObject go, MovementTypeSO so)
    {
        switch (so.enemyMovementIndex)
        {
            case Enums.MovementEnum.Type1:
                {
                    var movement = go.AddComponent<MoveHorizontalOscillate>();
                    movement.index = 1;
                    return movement;
                }

            case Enums.MovementEnum.Type2:
                {
                    var movement = go.AddComponent<MoveChaseAndExit>();
                    movement.index = 2;
                    return movement;
                }

            case Enums.MovementEnum.Type3:
                {
                    var movement = go.AddComponent<MoveHorizontalInOut>();
                    movement.index = 3;
                    return movement;
                }

            case Enums.MovementEnum.Type4:
                {
                    var movement = go.AddComponent<MoveStationary>();
                    movement.index = 4;
                    return movement;
                }

            case Enums.MovementEnum.Type5:
                {
                    var movement = go.AddComponent<MoveTopRightToBottomLeft>();
                    movement.index = 5;
                    return movement;
                }

            case Enums.MovementEnum.Type6:
                {
                    var movement = go.AddComponent<MoveTopLeftToBottomRight>();
                    movement.index = 6;
                    return movement;
                }

            case Enums.MovementEnum.Type7:
                {
                    var movement = go.AddComponent<MoveVerticalOnly>();
                    movement.index = 7;
                    return movement;
                }

            default:
                return null;
        }
    }

    SpaceShooterWeapon CreateWeapon(GameObject go, WeaponTypeSo so)
    {
        switch (so.Type)
        {
            case Enums.WeaponTypeEnum.Type1:
                {
                    var weapon = go.AddComponent<WeaponStraightDown>();
                    weapon.index = 1;
                    return weapon;
                }

            case Enums.WeaponTypeEnum.Type2:
                {
                    var weapon = go.AddComponent<WeaponDoubleDown>();
                    weapon.index = 2;
                    return weapon;
                }

            case Enums.WeaponTypeEnum.Type3:
                {
                    var weapon = go.AddComponent<WeaponAimPlayer>();
                    weapon.index = 3;
                    return weapon;
                }

            case Enums.WeaponTypeEnum.Type4:
                {
                    var weapon = go.AddComponent<WeaponDoubleAimPlayer>();
                    weapon.index = 4;
                    return weapon;
                }

            case Enums.WeaponTypeEnum.Type5:
                {
                    var weapon = go.AddComponent<WeaponRadial6>();
                    weapon.index = 5;
                    return weapon;
                }

            default:
                return null;
        }
    }
}