using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour
{
    public string nameScene;
    private string levelFileName, enemyFileName;
    public NarrativeConfigSO narrativeSO;
    private GameManager gameManager;

    public string LevelFileName { get => levelFileName; set => levelFileName = value; }
    public string EnemyFileName { get => enemyFileName; set => enemyFileName = value; }

    public static event LevelLoadEvent loadLevelEventHandler;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

        }
    }
}
