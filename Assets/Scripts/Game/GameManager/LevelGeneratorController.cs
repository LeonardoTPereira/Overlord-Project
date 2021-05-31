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
    public static event CreateEADungeonEvent createEADungeonEventHandler;

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
    }
    public void OnDisable()
    {
        Program.newEAGenerationEventHandler -= UpdateProgressBar;
    }

    public void CreateLevel()
    {
        int nRooms, nKeys, nLocks;
        float linearity;
        /*try
        {
            nRooms = int.Parse(inputFields["RoomsInputField"].text);
            nKeys = int.Parse(inputFields["KeysInputField"].text);
            nLocks = int.Parse(inputFields["LocksInputField"].text);
            linearity = float.Parse(inputFields["LinearityInputField"].text);
            fitness = new Fitness(nRooms, nKeys, nLocks, linearity);
        }
        catch (KeyNotFoundException)
        {
            Debug.LogWarning("Input Fields for Dungeon Generator incorrect. Using values from the Editor");
        }*/
        inputCanvas.SetActive(false);
        progressCanvas.SetActive(true);
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath +
        "\\Resources\\" + narrativeConfigSO.narrativeFileName + "\\Enemy");
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.*");
        string narrativeText = Resources.Load<TextAsset>(narrativeConfigSO.narrativeFileName + 
            "/Enemy/"+fileInfos[0].Name.Replace(".json", "")).text;
        JSonWriter.parametersMonsters parametersMonsters = JsonConvert.DeserializeObject<JSonWriter.parametersMonsters>(narrativeText);

        directoryInfo = new DirectoryInfo(Application.dataPath +
        "\\Resources\\" + narrativeConfigSO.narrativeFileName + "\\Dungeon");
        fileInfos = directoryInfo.GetFiles("*.*");
        narrativeText = Resources.Load<TextAsset>(narrativeConfigSO.narrativeFileName + 
            "/Dungeon/" + fileInfos[0].Name.Replace(".json", "")).text;
        JSonWriter.parametersDungeon parametersDungeon = JsonConvert.DeserializeObject<JSonWriter.parametersDungeon>(narrativeText);

        createEADungeonEventHandler?.Invoke(this, new CreateEADungeonEventArgs(parametersDungeon, parametersMonsters));
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
