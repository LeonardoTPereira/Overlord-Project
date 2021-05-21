using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour
{
    public string nameScene;
    LevelConfigSO levelConfig;
    private GameManager gameManager;

    public static event LevelLoadEvent loadLevelEventHandler;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            loadLevelEventHandler(this, new LevelLoadEventArgs(levelConfig.fileName, levelConfig.enemyDifficultyFile));
            SceneManager.LoadScene(nameScene);
        }
    }
}
