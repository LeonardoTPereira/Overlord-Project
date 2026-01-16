using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public int questID = -1;
    [SerializeField] private TextMeshProUGUI _questDescription;
    
    public void SetText(string text)
        { _questDescription.text = text; }
}
