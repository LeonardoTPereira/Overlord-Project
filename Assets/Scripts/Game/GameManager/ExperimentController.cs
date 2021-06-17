using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Enums;



public class ExperimentController : MonoBehaviour
{
    private class NarrativeClassPlaceholder
    {
        public string name;
    }

    private static readonly char SEPARATOR_CHARACTER = Path.DirectorySeparatorChar;
    private static readonly string EXPERIMENT_DIRECTORY = SEPARATOR_CHARACTER + "Experiment";
    private static string PROFILE_DIRECTORY;
    [SerializeField]
    private DungeonEntrance[] dungeonEntrances;
    private List<TextAsset> dungeonAssetsList, narrativeAssetsList;
    private List<string> enemyDirectoriesList;

    private void Awake()
    {
        dungeonAssetsList = null;
        narrativeAssetsList = null;
        enemyDirectoriesList = null;
    }

    private void OnEnable()
    {
        Manager.ProfileSelectedEventHandler += LoadDataForExperiment;
    }

    private void OnDisable()
    {
        Manager.ProfileSelectedEventHandler -= LoadDataForExperiment;
    }

    public void Start()
    {
        dungeonEntrances = FindObjectsOfType<DungeonEntrance>();
    }

    private void SetProfileDirectory(PlayerProfileEnum playerProfile)
    {
        PROFILE_DIRECTORY = EXPERIMENT_DIRECTORY + SEPARATOR_CHARACTER + playerProfile.ToString() + SEPARATOR_CHARACTER;
    }

    private void LoadDataForExperiment(object sender, ProfileSelectedEventArgs profileSelectedEventArgs)
    {
        SetProfileDirectory(profileSelectedEventArgs.PlayerProfile);
        NarrativeClassPlaceholder narrative = SelectNarrative();
        LoadAvailableLevelsForNarrative(narrative.name);
        LoadAvailableEnemiesForNarrative(narrative.name);

    }

    private NarrativeClassPlaceholder SelectNarrative()
    {
        if (narrativeAssetsList == null)
        {
            TextAsset[] narrativeAssets = LoadAllNarratives();
            narrativeAssetsList = narrativeAssets.ToList();
        }
        int selectedNarrativeIndex = Random.Range(0, narrativeAssetsList.Count);
        string narrativeContent = narrativeAssetsList[selectedNarrativeIndex].text;
        narrativeAssetsList.RemoveAt(selectedNarrativeIndex);
        return JsonConvert.DeserializeObject<NarrativeClassPlaceholder>(narrativeContent);
    }

    private void LoadAvailableLevelsForNarrative(string narrativeName)
    {
        if (dungeonAssetsList == null)
        {
            TextAsset[] dungeonAssets = LoadAllDungeons(narrativeName);
            dungeonAssetsList = dungeonAssets.ToList();
        }
        string[] levelFileNames = SelectDungeonsForThisRound();
        SetFilesToDungeonEntrances(levelFileNames);
    }

    private void LoadAvailableEnemiesForNarrative(string narrativeName)
    {
        if (enemyDirectoriesList == null)
        {
            string[] enemieDirectories = LoadAllEnemieDirectories(narrativeName);
            enemyDirectoriesList = enemieDirectories.ToList();
        }
        string[] enemyDirectoriesName = SelectEnemiesForThisRound();
        SetEnemiesToDungeonEntrances(enemyDirectoriesName);
    }

    private void SetFilesToDungeonEntrances(string []levelFileNames)
    {
        for (int i = 0; i < dungeonEntrances.Length; ++i)
        {
            dungeonEntrances[i].LevelFileName = levelFileNames[i];
        }
    }

    private void SetEnemiesToDungeonEntrances(string[] enemyFileNames)
    {
        for (int i = 0; i < dungeonEntrances.Length; ++i)
        {
            dungeonEntrances[i].EnemyFileName = enemyFileNames[i];
        }
    }

    private string[] SelectDungeonsForThisRound()
    {
        string[] levelFileNames = new string[dungeonEntrances.Length - 1];
        for (int i = 0; i < dungeonEntrances.Length; ++i)
        {
            int selectedDungeon = Random.Range(0, dungeonAssetsList.Count);
            levelFileNames[i] = dungeonAssetsList[selectedDungeon].name;
            dungeonAssetsList.RemoveAt(selectedDungeon);
        }
        return levelFileNames;
    }

    private string[] SelectEnemiesForThisRound()
    {
        string[] enemyDirectoryNames = new string[dungeonEntrances.Length - 1];
        for (int i = 0; i < dungeonEntrances.Length; ++i)
        {
            int selectedEnemy = Random.Range(0, enemyDirectoriesList.Count);
            enemyDirectoryNames[i] = enemyDirectoriesList[selectedEnemy];
            dungeonAssetsList.RemoveAt(selectedEnemy);
        }
        return enemyDirectoryNames;
    }

    private TextAsset[] LoadAllNarratives()
    {
        string narrativeDirectoryPath = PROFILE_DIRECTORY + "Narratives" + SEPARATOR_CHARACTER;
        return Resources.LoadAll<TextAsset>(narrativeDirectoryPath);
    }

    private TextAsset[] LoadAllDungeons(string narrativeDirectoryName)
    {
        string dungeonDirectoryPath = PROFILE_DIRECTORY + SEPARATOR_CHARACTER + "Dungeons" + SEPARATOR_CHARACTER + narrativeDirectoryName + SEPARATOR_CHARACTER;
        return Resources.LoadAll<TextAsset>(dungeonDirectoryPath);
    }

    private string[] LoadAllEnemieDirectories(string narrativeDirectoryName)
    {
        List<TextAsset[]> enemiesPerDirectory = new List<TextAsset[]>();
        string enemyDirectoryPath = PROFILE_DIRECTORY + SEPARATOR_CHARACTER + "Enemies" + SEPARATOR_CHARACTER + narrativeDirectoryName + SEPARATOR_CHARACTER;
        return Directory.GetDirectories(Application.dataPath + SEPARATOR_CHARACTER + enemyDirectoryPath);
    }

}



//This loads the narratives for creating dungeons in real time when needed
/*public void LoadAvailableNarrativesForProfile(PlayerProfileEnum playerProfile)
{
    // Get the directory separator
    char sep = Path.DirectorySeparatorChar;

    // Define the JSON file extension
    const string extension = ".json";

    DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + sep + "Resources" + sep + narrativeSO.narrativeFileName + sep + "Dungeon");
    FileInfo[] fileInfos = directoryInfo.GetFiles("*.*");
    string narrativeText = Resources.Load<TextAsset>(narrativeSO.narrativeFileName + sep + "Dungeon" + sep + fileInfos[0].Name.Replace(extension, "")).text;
    JSonWriter.parametersDungeon parametersDungeon = JsonConvert.DeserializeObject<JSonWriter.parametersDungeon>(narrativeText);

    //narrativeText = Resources.Load<TextAsset>(narrativeConfigSO.narrativeFileName + sep + "Dungeon" + sep + fileInfos[0].Name.Replace(extension, "")).text;
    TextAsset[] levelAssets = Resources.LoadAll<TextAsset>("Levels" + sep);
    string levelName = "R" + parametersDungeon.size + "-K" + parametersDungeon.nKeys + "-L" + parametersDungeon.nKeys + "-L" + parametersDungeon.linearity;
    Debug.Log(levelName);
    Debug.Log(levelAssets[0].name);
    TextAsset[] availableDungeons = levelAssets.Where(l => l.name.Contains(levelName)).ToArray();

    Debug.Log("DungeonFileName: " + availableDungeons[0].name);
    loadLevelEventHandler(this, new LevelLoadEventArgs(availableDungeons[0].name, "Enemies" + sep + "Hard" + sep));
    SceneManager.LoadScene("LevelWithEnemies");
}*/