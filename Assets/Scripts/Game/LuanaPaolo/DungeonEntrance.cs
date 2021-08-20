using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour
{
    public string nameScene;
    [SerializeField]
    private string levelFileName;
    private GameManager gameManager;

    public string LevelFileName { get => levelFileName; set => levelFileName = value; }

    public static event LevelLoadEvent loadLevelEventHandler;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            loadLevelEventHandler(this, new LevelLoadEventArgs(LevelFileName));
            SceneManager.LoadScene("LevelWithEnemies");
        }
    }
}
