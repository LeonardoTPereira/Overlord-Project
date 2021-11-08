using Game.GameManager;
using Game.EnemyGenerator;
using System.Linq;
using ScriptableObjects;
using UnityEngine;
using static Util.Enums;
using Newtonsoft.Json.Linq;
using Util;

public class EnemyLoader : MonoBehaviour
{

    private static readonly string ENEMY_FOLDER = "Enemies";

    [SerializeField]
    public EnemySO[] arena;
    [SerializeField]
    public EnemySO[] easy, medium, hard;
    public GameObject enemyPrefab;
    public GameObject barehandEnemyPrefab;
    public GameObject shooterEnemyPrefab;
    public GameObject bomberEnemyPrefab;
    public GameObject healerEnemyPrefab;

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
            case (int) EnemyTypeEnum.ARENA:
                enemyFolder += "Arena/";
                TextAsset[] enemies = Resources
                    .LoadAll(enemyFolder, typeof(TextAsset))
                    .Cast<TextAsset>().ToArray();
                arena = LoadEnemiesFromJSON(enemies, enemyFolder);
                ApplyDelegates(arena);
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
        if (currentEnemies[index].weapon.name == "None")
        {
            enemy = Instantiate(barehandEnemyPrefab, position, rotation);
        }
        else if (currentEnemies[index].weapon.name == "Bow")
        {
            enemy = Instantiate(shooterEnemyPrefab, position, rotation);
        }
        else if (currentEnemies[index].weapon.name == "BombThrower")
        {
            enemy = Instantiate(bomberEnemyPrefab, position, rotation);
        }
        else if (currentEnemies[index].weapon.name == "Cure")
        {
            enemy = Instantiate(healerEnemyPrefab, position, rotation);
        }
        else
        {
            enemy = Instantiate(enemyPrefab, position, rotation);
        }
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
            case (int) EnemyTypeEnum.ARENA:
                return arena;
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

    /// Load an array of enemies from JSON files and return an array of EnemySO.
    private EnemySO[] LoadEnemiesFromJSON(TextAsset[] jsons, string folder)
    {
        EnemySO[] enemies = new EnemySO[jsons.Length];
        for (int i = 0; i < jsons.Length; i++)
        {
            enemies[i] = LoadEnemyFromJSON(jsons[i], folder);
        }
        return enemies;
    }

    /// Load an enemy from a JSON file and return an EnemySO.
    private EnemySO LoadEnemyFromJSON(TextAsset json, string folder)
    {
        // Parse JSON
        JToken individual = JToken.Parse(json.text);
        JToken enemy = individual["enemy"];
        JToken weapon = individual["weapon"];
        int weaponIndex = (int) weapon["weaponType"];
        int movementIndex = (int) enemy["movementType"];
        // The `0` means that the enemy behavior is indifferent
        int behaviorIndex = 0;
        if (enemy["behaviorType"] != null)
        {
            behaviorIndex = (int) enemy["behaviorType"];
        }

        GameManagerSingleton gm = GameManagerSingleton.instance;

        // Convert JSON into Scriptable Object
        EnemySO asset = ScriptableObject.CreateInstance<EnemySO>();
        asset.Init(
            (int) enemy["health"],
            (int) enemy["strength"],
            (float) enemy["movementSpeed"],
            (float) enemy["activeTime"],
            (float) enemy["restTime"],
            gm.enemyComponents.weaponSet.Items[weaponIndex],
            gm.enemyComponents.movementSet.Items[movementIndex],
            gm.enemyComponents.behaviorSet.Items[behaviorIndex],
            (float) individual["fitness"],
            (float) enemy["attackSpeed"],
            (float) weapon["projectileSpeed"]
        );
        return asset;
    }
}
