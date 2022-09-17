using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

using Game.LevelSelection;
using Game.NarrativeGenerator.Quests;
using Game.Quests;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private InputActionAsset controls;
    private InputActionMap _inputActionMap;
    public InputAction toggleQuestUI;

    private QuestUIController _controller;
    private VisualElement _root;

    private QuestLineList currentQuestLines;

    private void Awake()
    {
        QuestController questController = FindObjectOfType<QuestController>();
        if ( questController == null )
            Destroy(gameObject);
        currentQuestLines = questController.QuestLines;

        _inputActionMap = controls.FindActionMap("UI");
        toggleQuestUI = _inputActionMap.FindAction("ToggleQuestUI");
        toggleQuestUI.performed += OnQuestUIToggle;
    }

    private void OnEnable()
    {
        UIDocument menu = GetComponent<UIDocument>();
        _root = menu.rootVisualElement;

        _controller = new(_root);
        _controller.RegisterTabCallbacks();
        _controller.ToggleQuestUI();
    }

    public void OnQuestUIToggle(InputAction.CallbackContext context)
    {
        PopulateLabels();
        _controller.ToggleQuestUI();
    }

    public void PopulateLabels()
    {
        List<string> questContents = new List<string>();
        questContents.Add("");
        questContents.Add("");

        foreach (var questLine in currentQuestLines.QuestLines)
        {
            questContents[0] += "\n - "+questLine.GetCurrentQuest().GetType().Name.Replace("QuestSo", "")+" "+questLine.GetCurrentQuest().ToString();
            foreach (var quest in questLine.GetCompletedQuests())
            {
                questContents[1] += "\n - "+quest.GetType().Name.Replace("QuestSo", "")+" "+quest.ToString();
            }
        }
        _controller.PopulateLabels(questContents);
    }
}
