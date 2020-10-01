using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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
    public delegate void SelectLevelButtonEvent(LevelConfigSO levelSO);
    public static event SelectLevelButtonEvent selectLevelButtonEvent;

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
        selectLevelButtonEvent += DisableOutline;
    }

    protected void OnDisable()
    {
        selectLevelButtonEvent -= DisableOutline;
    }

    protected void DisableOutline(LevelConfigSO level)
    {
        if(!level.Equals(levelConfigSO))
            outline.enabled = false;
    }

    void OnSelectLevel()
    {
        outline.enabled = true;
        selectLevelButtonEvent(levelConfigSO);
    }
}
