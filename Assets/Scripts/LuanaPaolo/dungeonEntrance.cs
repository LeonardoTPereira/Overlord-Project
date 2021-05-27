using Newtonsoft.Json;
using System.IO;
using System.Linq;
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
            "\\Resources\\" + narrativeSO.narrativeFileName + "\\Dungeon");
            FileInfo []fileInfos = directoryInfo.GetFiles("*.*");
            string narrativeText = Resources.Load<TextAsset>(narrativeSO.narrativeFileName +
                "/Dungeon/" + fileInfos[0].Name.Replace(".json", "")).text;
            JSonWriter.parametersDungeon parametersDungeon = JsonConvert.DeserializeObject<JSonWriter.parametersDungeon>(narrativeText);

            //narrativeText = Resources.Load<TextAsset>(narrativeConfigSO.narrativeFileName +
            //   "/Dungeon/" + fileInfos[0].Name.Replace(".json", "")).text;
            TextAsset []levelAssets = Resources.LoadAll<TextAsset>("Levels/");
            string levelName = "R" + parametersDungeon.size + "-K" + parametersDungeon.nKeys + "-L" + parametersDungeon.nKeys + "-L" + parametersDungeon.linearity;
            Debug.Log(levelName);
            Debug.Log(levelAssets[0].name);
            TextAsset []availableDungeons = levelAssets.Where(l => l.name.Contains(levelName)).ToArray();

            Debug.Log("DungeonFileName: " + availableDungeons[0].name);
            loadLevelEventHandler(this, new LevelLoadEventArgs(availableDungeons[0].name, "Enemies/Hard/"));
            SceneManager.LoadScene("LevelWithEnemies");
        }
    }
}
