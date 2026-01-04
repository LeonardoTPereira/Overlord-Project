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

    // Playable area
    const float MIN_X = -1.5f;
    const float MAX_X = 0.5f;
    const float MIN_Y = -1.0f;
    const float MAX_Y = 1.0f;

    // Spawn helpers
    const float OUT_BOTTOM_Y = 1.1f;
    const float TOP_SPAWN_MIN_Y = 0.7f;
    const float TOP_SPAWN_MAX_Y = 0.95f;

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
        Vector2 spawnPos = GetSpawnPosition(so.movement);
        var enemyGO = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        var enemy = enemyGO.GetComponent<SpaceShooterEnemy>();

        var movement = CreateMovement(enemyGO, so.movement);
        var weapon = CreateWeapon(enemyGO, so.weapon);

        enemy.Init(so, movement, weapon);
    }


    public static SpaceShooterMovement CreateMovement(GameObject go, MovementTypeSO so)
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

    public static SpaceShooterWeapon CreateWeapon(GameObject go, WeaponTypeSo so)
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

    Vector2 GetSpawnPosition(MovementTypeSO movement)
    {
        switch (movement.enemyMovementIndex)
        {
            // T1: Centro ± D no X, Y entre 0.7 e 0.95
            case Enums.MovementEnum.Type1:
                {
                    float D = 0.75f;
                    float x = Random.value > 0.5f ? D : -D;
                    float y = Random.Range(TOP_SPAWN_MIN_Y, TOP_SPAWN_MAX_Y);
                    return new Vector2(x, y);
                }

            // T2: Y entre 0.7 e 0.95, X livre no range jogável
            case Enums.MovementEnum.Type2:
                {
                    float x = Random.Range(MIN_X, MAX_X);
                    float y = Random.Range(TOP_SPAWN_MIN_Y, TOP_SPAWN_MAX_Y);
                    return new Vector2(x, y);
                }

            // T3: fora da tela embaixo
            case Enums.MovementEnum.Type3:
            case Enums.MovementEnum.Type4:
            case Enums.MovementEnum.Type7:
                {
                    float x = Random.Range(MIN_X, MAX_X);
                    return new Vector2(x, OUT_BOTTOM_Y);
                }

            // T5: canto superior direito → inferior esquerdo
            case Enums.MovementEnum.Type5:
                return new Vector2(MIN_X, MAX_Y);

            // T6: canto superior esquerdo → inferior direito
            case Enums.MovementEnum.Type6:
                return new Vector2(MAX_X, MAX_Y);

            default:
                return Vector2.zero;
        }
    }
}