using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestUI : MonoBehaviour
{
    private QuestUIController controller;

    private void OnEnable()
    {
        UIDocument menu = GetComponent<UIDocument>();
        VisualElement root = menu.rootVisualElement;

        controller = new(root);

        controller.RegisterTabCallbacks();
    }
}
