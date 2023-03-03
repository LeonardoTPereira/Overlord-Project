using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// This is the heart of the Lifebar_UI
// The Heart have the 'heart' uss StyleSheet and an icon ( empty, half or full heart sprite)
public class Heart : VisualElement
{
    public Heart(Sprite icon)
    {
        AddToClassList("heart");                                // Add the '.heart' uss styles
        UpdateIcon(icon);
    }

    public void UpdateIcon(Sprite icon)
    {
        this.style.backgroundImage = new StyleBackground(icon); // Add the heart icon
    }
}
