using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FormQuestionData : ScriptableObject {
    [TextArea]
    public string question;
    public string description;
}
