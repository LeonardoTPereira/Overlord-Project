using UnityEngine;

[CreateAssetMenu]
public class FormQuestionData : ScriptableObject
{
    [TextArea]
    public string question;
    public string description;
}
