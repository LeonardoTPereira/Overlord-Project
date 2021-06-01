using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Enums;

public class ExperimentController : MonoBehaviour
{
    private static readonly char SEPARATOR_CHARACTER = Path.DirectorySeparatorChar;
    private static readonly string EXPERIMENT_DIRECTORY = SEPARATOR_CHARACTER + "Experiment";
    DungeonEntrance[] dungeonEntrances;

    public void Start()
    {
        dungeonEntrances = FindObjectsOfType<DungeonEntrance>();
    }

    public void LoadAvailableLevelsForProfile(PlayerProfileEnum playerProfile)
    {
        TextAsset[] dungeonAssets = LoadAllDungeons(playerProfile);
        List<TextAsset> dungeonAssetsList = dungeonAssets.ToList();
        string[] levelFileNames = SelectDungeonsForThisRound(dungeonAssetsList);
        SetFilesToDungeonEntrances(levelFileNames);
    }

    private void SetFilesToDungeonEntrances(string []levelFileNames)
    {
        for (int i = 0; i < dungeonEntrances.Length; ++i)
        {
            dungeonEntrances[i].LevelFileName = levelFileNames[i];
        }
    }

    private string[] SelectDungeonsForThisRound(List<TextAsset> dungeonAssetsList)
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

    private TextAsset[] LoadAllDungeons(PlayerProfileEnum playerProfile)
    {
        string dungeonDirectoryPath = EXPERIMENT_DIRECTORY + SEPARATOR_CHARACTER + playerProfile.ToString() + SEPARATOR_CHARACTER + "Dungeons" + SEPARATOR_CHARACTER;
        TextAsset[] dungeonAssets = Resources.LoadAll<TextAsset>(dungeonDirectoryPath);
        return dungeonAssets;
    }



}