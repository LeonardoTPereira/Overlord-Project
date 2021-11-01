using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Maestro;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour
{
    public static event ProfileSelectedEvent ProfileSelectedEventHandler;

    [SerializeField, MustBeAssigned]
    private PlayerProfileToQuestLinesDictionarySO playerProfileToQuestLinesDictionarySo;
    private List<QuestLine> questLineListForExperiment;

    private PlayerProfile selectedPlayerProfile;
    [SerializeField]
    private DungeonEntrance[] dungeonEntrances;

    private void OnEnable()
    {
        Manager.ProfileSelectedEventHandler += LoadDataForExperiment;
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        Manager.ProfileSelectedEventHandler -= LoadDataForExperiment;
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    IEnumerator WaitForProfileToBeLoadedAndSelectNarratives(Scene scene)
    {
        yield return new WaitUntil(() => CanLoadNarrativesToDungeonEntrances(scene));
        SelectAndSetNarrativesToDungeonEntrances();
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(WaitForProfileToBeLoadedAndSelectNarratives(scene));
    }

    private bool CanLoadNarrativesToDungeonEntrances(Scene scene)
    {
        return scene.name == "Overworld";
    }

    private void SelectAndSetNarrativesToDungeonEntrances()
    {

        QuestLine selectedQuestLine = questLineListForProfile.GetRandomQuestLine();
        dungeonEntrances = FindObjectsOfType<DungeonEntrance>();
        int nDungeonEntraces = dungeonEntrances.Length;
        Debug.Log("Dungeon Entrances Found: " + nDungeonEntraces);
        if (nChosenNarratives < nDungeonEntraces)
        {
            for (int i = nChosenNarratives; i < nDungeonEntraces; i++)
            {
                chosenNarratives.Add(null);
            }
        }
        for (int i = 0; i < dungeonEntrances.Length; ++i)
        {
            if (chosenNarratives[i] == null)
                chosenNarratives[i] = GetRandomDungeonFile();
            dungeonEntrances[i].LevelFileName = chosenNarratives[i];
            Debug.Log("Dungeon Entrance Filename: " + dungeonEntrances[i].LevelFileName);
        }
    }

    private void SetProfileDirectory(PlayerProfile playerProfile)
    {
        questLineListForExperiment = playerProfileToQuestLinesDictionarySo.QuestLinesForProfile[
            selectedPlayerProfile.PlayerProfileEnum.ToString()].QuestLines.Select(questLine => new QuestLine()).ToList();
    }

    private void LoadNarrativesForProfile()
    {
        Debug.Log("Application data path: " + Application.dataPath);
        //narrativeDirectories = Directory.GetDirectories(Application.dataPath + SEPARATOR_CHARACTER + "Resources" + SEPARATOR_CHARACTER + PROFILE_DIRECTORY).ToList();
        narrativeDirectories = narrativeFiles.NarrativeFolders;
        Debug.Log("Inside Load Narratives For Profile: "+narrativeDirectories); 
        GetAllDungeonsForProfile();
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

    private void GetAllDungeonsForProfile()
    {
        foreach (var directory in narrativeDirectories)
        {
            string relativePath = PROFILE_DIRECTORY+directory;
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

    private ParametersDungeon GetDungeonParametersJSON(string narrativeDirectory, string relativePath)
    {
        // Define the JSON file extension
        const string extension = ".json";
        //DirectoryInfo directoryInfo = new DirectoryInfo(narrativeDirectory + SEPARATOR_CHARACTER + "Dungeon");
        //FileInfo[] fileInfos = directoryInfo.GetFiles("*.json");
        Debug.Log("Path: " + PROFILE_DIRECTORY + narrativeDirectory + SEPARATOR_CHARACTER + "Dungeon");
        TextAsset []dungeonsAssets = Resources.LoadAll<TextAsset>(PROFILE_DIRECTORY + narrativeDirectory + SEPARATOR_CHARACTER + "Dungeon");
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

    private void LoadDataForExperiment(object sender, ProfileSelectedEventArgs profileSelectedEventArgs)
    {
        Debug.Log("Loading Data For Experiment. Profile: " + profileSelectedEventArgs.PlayerProfile.ToString());
        PlayerProfile selectedProfile;
        if (UnityEngine.Random.Range(0, 100) < 50)
        {
            selectedProfile = profileSelectedEventArgs.PlayerProfile;
        }
        else
        {
            selectedProfile = new PlayerProfile();
            do
            {
                selectedProfile.PlayerProfileEnum = (PlayerProfile.PlayerProfileCategory)UnityEngine.Random.Range(0, 4);
            } while (selectedProfile.PlayerProfileEnum == profileSelectedEventArgs.PlayerProfile.PlayerProfileEnum);
        }
        ProfileSelectedEventHandler?.Invoke(null, new ProfileSelectedEventArgs(selectedProfile));
        SetProfileDirectory(selectedProfile);
        LoadNarrativesForProfile();
    }

}