using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NarrativeConfigSO", menuName = "Overlord-Project/Narrative-Generator/Other/NarrativeConfigSO")]
    public class NarrativeConfigSO : ScriptableObject
    {
        public string narrativeFileName;
    }
}