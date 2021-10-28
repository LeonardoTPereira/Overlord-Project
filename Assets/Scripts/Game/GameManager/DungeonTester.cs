using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;
using static Util;

public class DungeonTester : MonoBehaviour
{
    [SerializeField]
    private DungeonFilesDictionarySO dungeonList;
    public static event LevelLoadEvent loadLevelEventHandler;
    public string levelFileName;
    private static readonly string EXPERIMENT_DIRECTORY = "Experiment";
    private string PROFILE_DIRECTORY;
    private List<string> narrativeDirectories = new List<string>();
    private List<string> dungeons = new List<string>();
    private List<string> chosenNarratives = new List<string>();
    [SerializeField]
    private NarrativeFilesRuntimeSetSO narrativeFilesSet;
    private NarrativeFilesSO narrativeFiles;
    public DungeonFileSO dungeonFileSO;

    private void Start()
    {
        //LoadDataForExperiment();
        /*DungeonFileSO dungeonFileSO;

        dungeonFileSO = ScriptableObject.CreateInstance<DungeonFileSO>();
        dungeonFileSO.Init(dungeonFile);
        Debug.Log(jsonContent);
#if UNITY_EDITOR
        AssetDatabase.CreateAsset(dungeonFileSO, "Assets/Resources/" + levelFileName + ".asset");
#endif*/
        loadLevelEventHandler(this, new LevelLoadEventArgs(dungeonFileSO));
        SceneManager.LoadScene("LevelWithEnemies");
    }

    private string GetDungeonFileForNarrative(string narrativeDirectory)
    {
        string relativePath = narrativeDirectory.Substring(narrativeDirectory.IndexOf("Experiment"));
        ParametersDungeon parametersDungeon = GetDungeonParametersJSON(narrativeDirectory, relativePath);
        return GetDungeonFile(relativePath, parametersDungeon);
    }

    private string GetDungeonFile(string relativePath, ParametersDungeon parametersDungeon)
    {
        TextAsset[] levelAssets = Resources.LoadAll<TextAsset>(relativePath + SEPARATOR_CHARACTER + "Levels" + SEPARATOR_CHARACTER);
        string levelName = "R" + parametersDungeon.size + "-K" + parametersDungeon.nKeys + "-L" + parametersDungeon.nKeys + "-L" + parametersDungeon.linearity;
        TextAsset[] availableDungeons = levelAssets.Where(l => l.name.Contains(levelName)).ToArray();
        int selectedDungeonIndex = UnityEngine.Random.Range(0, availableDungeons.Length);
        return relativePath + SEPARATOR_CHARACTER + "Levels" + SEPARATOR_CHARACTER + availableDungeons[selectedDungeonIndex].name;
    }

    private ParametersDungeon GetDungeonParametersJSON(string narrativeDirectory, string relativePath)
    {
        // Define the JSON file extension
        const string extension = ".json";
        //DirectoryInfo directoryInfo = new DirectoryInfo(narrativeDirectory + SEPARATOR_CHARACTER + "Dungeon");
        //FileInfo[] fileInfos = directoryInfo.GetFiles("*.json");
        Debug.Log("Path: " + PROFILE_DIRECTORY + narrativeDirectory + SEPARATOR_CHARACTER + "Dungeon");
        TextAsset[] dungeonsAssets = Resources.LoadAll<TextAsset>(PROFILE_DIRECTORY + narrativeDirectory + SEPARATOR_CHARACTER + "Dungeon");
        Debug.Log("Assets: " + dungeonsAssets);
        TextAsset selectedDungeon = dungeonsAssets[UnityEngine.Random.Range(0, dungeonsAssets.Length)];
        //string dungeonFilePath = relativePath + SEPARATOR_CHARACTER + "Dungeon" + SEPARATOR_CHARACTER + fileInfos[dungeonFileIndex].Name.Replace(extension, "");
        //Debug.Log(dungeonFilePath);
        //string dungeonFileContent = Resources.Load<TextAsset>(dungeonFilePath).text;
        ParametersDungeon parametersDungeon = JsonConvert.DeserializeObject<ParametersDungeon>(selectedDungeon.text);
        return parametersDungeon;
    }

    private string GetRandomNarrativeDirectoryAndRemoveFromList()
    {
        int nAvailableNarrativesForProfile = narrativeDirectories.Count;
        int selectedNarrativeIndex = UnityEngine.Random.Range(0, nAvailableNarrativesForProfile);
        string selectedNarrative = narrativeDirectories[selectedNarrativeIndex];
        narrativeDirectories.RemoveAt(selectedNarrativeIndex);
        return selectedNarrative;
    }

    private void GetAllDungeonsForProfile()
    {
        foreach (var directory in narrativeDirectories)
        {
            string relativePath = PROFILE_DIRECTORY + directory;
            ParametersDungeon parametersDungeon = GetDungeonParametersJSON(directory, relativePath);
            GetAllDungeons(relativePath, parametersDungeon);
        }
    }

    private void GetAllDungeons(string relativePath, ParametersDungeon parametersDungeon)
    {
        TextAsset[] levelAssets = Resources.LoadAll<TextAsset>(relativePath + SEPARATOR_CHARACTER + "Levels" + SEPARATOR_CHARACTER);
        string levelName = "R" + parametersDungeon.size + "-K" + parametersDungeon.nKeys + "-L" + parametersDungeon.nKeys + "-L" + parametersDungeon.linearity;
        TextAsset[] availableDungeons = levelAssets.Where(l => l.name.Contains(levelName)).ToArray();
        foreach (var dungeon in availableDungeons)
        {
            Debug.Log(dungeon.name);
            dungeons.Add(relativePath + SEPARATOR_CHARACTER + "Levels" + SEPARATOR_CHARACTER + dungeon.name);
        }
    }


    private void SetProfileDirectory(PlayerProfileEnum playerProfile)
    {
        PROFILE_DIRECTORY = EXPERIMENT_DIRECTORY + SEPARATOR_CHARACTER + playerProfile.ToString() + SEPARATOR_CHARACTER;
        narrativeFiles = narrativeFilesSet.GetNarrativesFromProfile(playerProfile.ToString());
    }

    private void LoadNarrativesForProfile()
    {
        Debug.Log("Application data path: " + Application.dataPath);
        //narrativeDirectories = Directory.GetDirectories(Application.dataPath + SEPARATOR_CHARACTER + "Resources" + SEPARATOR_CHARACTER + PROFILE_DIRECTORY).ToList();
        narrativeDirectories = narrativeFiles.NarrativeFolders;
        Debug.Log("Inside Load Narratives For Profile: " + narrativeDirectories);
        GetAllDungeonsForProfile();
    }
}
