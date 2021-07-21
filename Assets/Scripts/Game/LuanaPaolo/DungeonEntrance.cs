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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Get the directory separator
            char sep = Path.DirectorySeparatorChar;

            string[] directories = Directory.GetDirectories(Application.dataPath + sep + "Resources" + sep + narrativeSO.narrativeFileName);
            int nNarrativesForProfile = directories.Length;
            string selectedNarrative = directories[Random.Range(0, nNarrativesForProfile)];

            string relativePath = selectedNarrative.Substring(selectedNarrative.IndexOf(narrativeSO.narrativeFileName));

            // Define the JSON file extension
            const string extension = ".json";
            DirectoryInfo directoryInfo = new DirectoryInfo(selectedNarrative + sep + "Dungeon");
            FileInfo[] fileInfos = directoryInfo.GetFiles("*.*");
            string narrativeText = Resources.Load<TextAsset>(relativePath + sep + "Dungeon" + sep + fileInfos[0].Name.Replace(extension, "")).text;
            JSonWriter.ParametersDungeon parametersDungeon = JsonConvert.DeserializeObject<JSonWriter.ParametersDungeon>(narrativeText);

            //narrativeText = Resources.Load<TextAsset>(narrativeConfigSO.narrativeFileName + sep + "Dungeon" + sep + fileInfos[0].Name.Replace(extension, "")).text;
            TextAsset[] levelAssets = Resources.LoadAll<TextAsset>("Levels" + sep);
            string levelName = "R" + parametersDungeon.size + "-K" + parametersDungeon.nKeys + "-L" + parametersDungeon.nKeys + "-L" + parametersDungeon.linearity;
            //Debug.Log(levelName);
            //Debug.Log(levelAssets[0].name);
            TextAsset[] availableDungeons = levelAssets.Where(l => l.name.Contains(levelName)).ToArray();

            //Debug.Log("DungeonFileName: " + availableDungeons[0].name);
            loadLevelEventHandler(this, new LevelLoadEventArgs(availableDungeons[0].name, "Enemies" + sep + "Hard" + sep));
            SceneManager.LoadScene("LevelWithEnemies");
        }
    }
}
