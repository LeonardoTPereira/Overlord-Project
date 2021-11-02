using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Game.GameManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour
{
    public string nameScene;

    [field: SerializeField] public DungeonFileSO DungeonFileSo { get; set; }
    private GameManagerSingleton _gameManagerSingleton;
    
    public static event LevelLoadEvent loadLevelEventHandler;

    void Start()
    {
        _gameManagerSingleton = FindObjectOfType<GameManagerSingleton>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            loadLevelEventHandler(this, new LevelLoadEventArgs(DungeonFileSo));
            SceneManager.LoadScene("LevelWithEnemies");
        }
    }
}
