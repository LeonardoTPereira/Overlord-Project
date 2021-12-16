using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButtonBHV : MonoBehaviour
{
    [SerializeField]
    protected LevelConfigSO levelConfigSO;
    TextMeshProUGUI buttonName;
    Button button;
    Outline outline;
    public static event LevelSelectEvent selectLevelButtonEventHandler;

    // Start is called before the first frame update
    void Start()
    {
        buttonName = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        outline = GetComponent<Outline>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnSelectLevel);
        buttonName.text = levelConfigSO.levelName;
        outline.enabled = false;
    }
    protected void OnEnable()
    {
        selectLevelButtonEventHandler += DisableOutline;
    }

    protected void OnDisable()
    {
        selectLevelButtonEventHandler -= DisableOutline;
    }

    protected void DisableOutline(object sender, LevelSelectEventArgs args)
    {
        if (!args.LevelSO.Equals(levelConfigSO))
            outline.enabled = false;
    }

    void OnSelectLevel()
    {
        outline.enabled = true;
        selectLevelButtonEventHandler(this, new LevelSelectEventArgs(levelConfigSO));
    }
}
