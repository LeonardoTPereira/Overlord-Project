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

    /// Load the level from the given filename.
    public static void LoadLevel(object from, string filename)
    {
        loadLevelEventHandler(from, new LevelLoadEventArgs(filename));
        SceneManager.LoadScene("LevelWithEnemies");
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadLevel(this, LevelFileName);
        }
    }
}
