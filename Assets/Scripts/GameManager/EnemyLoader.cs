using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyGenerator;
using System.Linq;

public class EnemyLoader : MonoBehaviour
{
    [SerializeField]
    public EnemySO[] bestEnemies;
    public GameObject enemyPrefab, bomberEnemyPrefab;

    void Start()
    {
    }

    public void LoadEnemies(int difficulty)
    {
        string foldername = "Enemies/";
        switch (difficulty)
        {
            case 0:
                foldername += "Easy/";
                break;
            case 1:
                foldername += "Medium/";
                break;
            case 2:
                foldername += "Hard/";
                break;
        }
        bestEnemies = Resources.LoadAll(foldername, typeof(EnemySO)).Cast<EnemySO>().ToArray();
        ApplyDelegates();
    }

    public GameObject InstantiateEnemyWithIndex(int index, Vector3 position, Quaternion rotation)
    {
        //Debug.Log("Begin instantiating");
        GameObject enemy;
        if(bestEnemies[index].weapon.name == "BombThrower")
            enemy = Instantiate(bomberEnemyPrefab, position, rotation);
        else
            enemy = Instantiate(enemyPrefab, position, rotation);
        enemy.GetComponent<EnemyController>().LoadEnemyData(bestEnemies[index], index);
        return enemy;
    }
    private void ApplyDelegates()
    {
        foreach (EnemySO enemy in bestEnemies)
        {
            enemy.movement.movementType = GetMovementType(enemy.movement.enemyMovementIndex);
        }
    }
    public MovementType GetMovementType(MovementEnum moveTypeEnum)
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
