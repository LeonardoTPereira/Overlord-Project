using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class Heart : VisualElement
{        
    public Heart(Sprite icon)
    {
        AddToClassList("heart");                                // Add the '.heart' uss styles
        UpdateIcon(Sprite icon); 
    }

    private void UpdateIcon(Sprite icon)
    {
        this.style.backgroundImage = new StyleBackground(icon); // Add the heart icon
    }
}
