using Game.NarrativeGenerator.Quests;
using Game.Quests;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private InputActionReference interactAction;

    private QuestUIController _controller;
    private VisualElement _root;

    private QuestLineList currentQuestLines;

    private void Awake()
    {
        QuestController questController = FindObjectOfType<QuestController>();
        if ( questController == null )
            Destroy(gameObject);
        currentQuestLines = questController.QuestLines;
        
    }

    private void OnEnable()
    {
        interactAction.action.performed += OnQuestUIToggle;
        UIDocument menu = GetComponent<UIDocument>();
        _root = menu.rootVisualElement;

        _controller = new(_root);
        _controller.RegisterTabCallbacks();
        _controller.ToggleQuestUI();
    }

    private void OnDisable()
    {
        interactAction.action.performed -= OnQuestUIToggle;
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
            if (questLine.GetCurrentQuest() != null)
            {
                questLine.GetCurrentQuest().CreateQuestString();
                //questContents[0] += "\n - "+questLine.GetCurrentQuest().GetType().Name.Replace("QuestSo", "") + " " + RemoveAngleBracketContent(questLine.GetCurrentQuest().ToString());
                questContents[0] += "\n - " + " " + RemoveAngleBracketContent(questLine.GetCurrentQuest().ToString());
            }
            foreach (var quest in questLine.GetCompletedQuests())
            {
                //questContents[1] += "\n - "+quest.GetType().Name.Replace("QuestSo", "") + " " + RemoveAngleBracketContent(quest.ToString());
                questContents[1] += "\n - " + " " + RemoveAngleBracketContent(quest.ToString());
            }
        }
        _controller.PopulateLabels(questContents);
    }
        private string RemoveAngleBracketContent(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Regex: "<[^>]*>" matches "<" followed by any number of non-">" chars, ending with ">"
        return System.Text.RegularExpressions.Regex.Replace(input, "<[^>]*>", string.Empty).Trim();
    }

}
