using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class QuestUI : MonoBehaviour
{
    private QuestUIController _controller;
    private VisualElement _root;

    private void OnEnable()
    {
        UIDocument menu = GetComponent<UIDocument>();
        _root = menu.rootVisualElement;

        _controller = new(_root);
        _controller.RegisterTabCallbacks();
    }

    public void ToggleQuestUI(InputAction.CallbackContext context)
    {
        Debug.Log("GOT INPUT");
        _controller.ToggleQuestUI();
    }
}
