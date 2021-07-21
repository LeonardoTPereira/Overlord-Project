using LevelGenerator;
using MyBox;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Program), typeof(NarrativeConfigSO))]
public class LevelGeneratorController : MonoBehaviour, IMenuPanel
{

    public static class NarrativeFileTypeString
    {
        public const string ENEMY = "Enemy";
        public const string ITEM = "Item";
        public const string NPC = "NPC";
        public const string DUNGEON = "Dungeon";
    }
    private char separator = Path.DirectorySeparatorChar;

    public static event CreateEADungeonEvent createEADungeonEventHandler;
    private string playerProfile;

    protected Dictionary<string, TMP_InputField> inputFields;
    [SerializeField]
    protected GameObject progressCanvas, inputCanvas;
    protected TextMeshProUGUI progressTextUI;
    [SerializeField, Scene]
    protected string levelToLoad;
    protected string progressText;

    [Separator("Fitness Parameters to Create Dungeons")]
    [SerializeField]
    protected Fitness fitness;
    [SerializeField]
    protected NarrativeConfigSO narrativeConfigSO;

    public void Awake()
    {
        inputCanvas.SetActive(true);
        progressCanvas.SetActive(true);
        Debug.LogWarning(progressCanvas.transform.Find("ProgressPanel/ProgressText"));
        progressTextUI = progressCanvas.transform.Find("ProgressPanel/ProgressText").GetComponent<TextMeshProUGUI>();
        inputFields = inputCanvas.GetComponentsInChildren<TMP_InputField>().ToDictionary(key => key.name, inputFieldObj => inputFieldObj);
        progressCanvas.SetActive(false);
    }

    public void OnEnable()
    {
        Program.newEAGenerationEventHandler += UpdateProgressBar;
        Manager.ProfileSelectedEventHandler += CreateLevelFromNarrative;
    }
    public void OnDisable()
    {
        Program.newEAGenerationEventHandler -= UpdateProgressBar;
        Manager.ProfileSelectedEventHandler -= CreateLevelFromNarrative;
    }

    public void CreateLevelFromNarrative()
    {
        inputCanvas.SetActive(false);
        progressCanvas.SetActive(true);
        string selectedNarrative = GetNarrativePath();

        JSonWriter.ParametersMonsters parametersMonsters
            = GetJSONData<JSonWriter.ParametersMonsters>(NarrativeFileTypeString.ENEMY, selectedNarrative);
        JSonWriter.ParametersItems parametersItems
            = GetJSONData<JSonWriter.ParametersItems>(NarrativeFileTypeString.ITEM, selectedNarrative);
        JSonWriter.ParametersNpcs parametersNpcs
            = GetJSONData<JSonWriter.ParametersNpcs>(NarrativeFileTypeString.NPC, selectedNarrative);
        JSonWriter.ParametersDungeon parametersDungeon
            = GetJSONData<JSonWriter.ParametersDungeon>(NarrativeFileTypeString.DUNGEON, selectedNarrative);

        createEADungeonEventHandler?.Invoke(this, new CreateEADungeonEventArgs(parametersDungeon,
            parametersMonsters, parametersItems, parametersNpcs));
    }

    private string GetNarrativePath()
    {
        string directoryPath = $"{Application.dataPath}{separator}Resources{separator}{playerProfile}";
        string[] directories = Directory.GetDirectories(directoryPath);
        int nNarrativesForProfile = directories.Length;
        string selectedNarrative = directories[Random.Range(0, nNarrativesForProfile)];
        return selectedNarrative;
    }

    public void CreateLevelFromNarrative(object sender, ProfileSelectedEventArgs eventArgs)
    {
        playerProfile = eventArgs.PlayerProfile.ToString();
        CreateLevelFromNarrative();
    }

        public void CreateLevelFromInput()
    {
        int nRooms, nKeys, nLocks;
        float linearity;
        try
        {
            nRooms = int.Parse(inputFields["RoomsInputField"].text);
            nKeys = int.Parse(inputFields["KeysInputField"].text);
            nLocks = int.Parse(inputFields["LocksInputField"].text);
            linearity = float.Parse(inputFields["LinearityInputField"].text);
            fitness = new Fitness(nRooms, nKeys, nLocks, linearity);
            createEADungeonEventHandler?.Invoke(this, new CreateEADungeonEventArgs(fitness));
        }
        catch (KeyNotFoundException)
        {
            Debug.LogWarning("Input Fields for Dungeon Generator incorrect. Using values from the Editor");
        }
        inputCanvas.SetActive(false);
        progressCanvas.SetActive(true);
    }

    private T GetJSONData<T>(string narrativeType, string narrativePath)
    {
        string dataPath = narrativePath + separator + narrativeType;
        string relativePath = dataPath.Substring(dataPath.IndexOf(playerProfile));
        TextAsset []files = Resources.LoadAll<TextAsset>(relativePath);
        int nFiles = files.Length;
        TextAsset selectedFile = files[Random.Range(0, nFiles)];
        return JsonConvert.DeserializeObject<T>(selectedFile.text);
    }

    public void UpdateProgressBar(object sender, NewEAGenerationEventArgs eventArgs)
    {
        progressText = eventArgs.CompletionRate.ToString() + "%";
        UnityMainThreadDispatcher.Instance().Enqueue(() => progressTextUI.text = progressText);
    }

    public void GoToNext()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void GoToPrevious()
    {
        inputCanvas.SetActive(true);
        progressCanvas.SetActive(false);
    }
}
