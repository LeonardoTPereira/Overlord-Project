using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class Heart : VisualElement
{
    private const int EMPTY_HEART = 0;
    private const int HALF_HEART = 1;
    private const int FULL_HEART = 2;

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
