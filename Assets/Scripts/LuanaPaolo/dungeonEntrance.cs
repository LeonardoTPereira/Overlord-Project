using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour
{
    public string nameScene;
    public NarrativeConfigSO narrativeSO;
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
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + 
                "\\Resources\\NarrativeJSon" + narrativeSO.narrativeFileName + "\\Dungeon");
            FileInfo[] fileInfos = directoryInfo.GetFiles("*.*");
            loadLevelEventHandler(this, new LevelLoadEventArgs(
                fileInfos[0].FullName,
                Application.dataPath + "\\Resources\\NarrativeJSon" + narrativeSO.narrativeFileName + "\\Enemy"));
            SceneManager.LoadScene("LevelWithEnemies");
            SceneManager.LoadScene(nameScene);
        }
    }
}
