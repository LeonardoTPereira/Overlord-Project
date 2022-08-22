using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DragAndDropProfiles
{
    public class DragAndDropBehaviour : MonoBehaviour
    {
        [SerializeField]
        private UIDocument m_UIDocument;

        private void OnEnable()
        {
            DragAndDropManipulator manipulator = new(m_UIDocument.rootVisualElement.Q<VisualElement>("object"));
        }
    }
}