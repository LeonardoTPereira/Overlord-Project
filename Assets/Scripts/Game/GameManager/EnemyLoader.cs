using EnemyGenerator;
using System.Linq;
using ScriptableObjects;
using UnityEngine;
using Util;
using static Util.Enums;

public class EnemyLoader : MonoBehaviour
{

    private static readonly string ENEMY_FOLDER = "Enemies";

    [SerializeField]
    public EnemySO[] easy, medium, hard;
    public GameObject enemyPrefab, bomberEnemyPrefab;

    public void LoadEnemies(int enemyType)
    {
        GetEnemyFilenameFromType(enemyType);
    }

    private void GetEnemyFilenameFromType(int enemyType)
    {
        string enemyFolder = ENEMY_FOLDER + "/";
        switch (enemyType)
        {
            case (int)EnemyTypeEnum.EASY:
                enemyFolder += "Easy/";
                easy = Resources.LoadAll(enemyFolder, typeof(EnemySO)).Cast<EnemySO>().ToArray();
                ApplyDelegates(easy);
                break;
            case (int)EnemyTypeEnum.MEDIUM:
                enemyFolder += "Medium/";
                medium = Resources.LoadAll(enemyFolder, typeof(EnemySO)).Cast<EnemySO>().ToArray();
                ApplyDelegates(medium);
                break;
            case (int)EnemyTypeEnum.HARD:
                enemyFolder += "Hard/";
                hard = Resources.LoadAll(enemyFolder, typeof(EnemySO)).Cast<EnemySO>().ToArray();
                ApplyDelegates(hard);
                break;
        }
    }

    public int GetRandomEnemyIndex(int enemyType)
    {
        EnemySO[] currentEnemies = GetEnemiesFromType(enemyType);
        return Random.Range(0, currentEnemies.Length);
    }

    public GameObject InstantiateEnemyWithIndex(int index, Vector3 position, Quaternion rotation, int enemyType)
    {
        Debug.Log("Index: "+index);
        EnemySO[] currentEnemies = GetEnemiesFromType(enemyType);
        GameObject enemy;
        if (currentEnemies[index].weapon.name == "BombThrower")
            enemy = Instantiate(bomberEnemyPrefab, position, rotation);
        else
            enemy = Instantiate(enemyPrefab, position, rotation);
        enemy.GetComponent<EnemyController>().LoadEnemyData(currentEnemies[index], index);
        return enemy;
    }

    private EnemySO[] GetEnemiesFromType(int enemyType)
    {
        Debug.Log("Enemy Type: " + enemyType);
        switch(enemyType)
        {
            case (int)EnemyTypeEnum.EASY:
                return easy;
                break;
            case (int)EnemyTypeEnum.MEDIUM:
                return medium;
                break;
            case (int)EnemyTypeEnum.HARD:
                return hard;
                break;
        }
        return medium;
    }

    private void ApplyDelegates(EnemySO []enemies)
    {
        foreach (EnemySO enemy in enemies)
        {
            enemy.movement.movementType = GetMovementType(enemy.movement.enemyMovementIndex);
        }
    }
    public MovementType GetMovementType(Enums.MovementEnum moveTypeEnum)
    {
        switch (moveTypeEnum)
        {
            case MovementEnum.None:
                return EnemyMovement.NoMovement;
            case MovementEnum.Random:
                return EnemyMovement.MoveRandomly;
            case MovementEnum.Flee:
                return EnemyMovement.FleeFromPlayer;
            case MovementEnum.Follow:
                return EnemyMovement.FollowPlayer;
            case MovementEnum.Follow1D:
                return EnemyMovement.FollowPlayer1D;
            case MovementEnum.Random1D:
                return EnemyMovement.MoveRandomly1D;
            case MovementEnum.Flee1D:
                return EnemyMovement.FleeFromPlayer1D;
            default:
                Debug.Log("No Movement Attached to Enemy");
                return null;
        }
    }
}
