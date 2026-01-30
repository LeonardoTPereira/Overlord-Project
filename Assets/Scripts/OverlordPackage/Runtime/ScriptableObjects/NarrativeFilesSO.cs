using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NarrativeFilesSO", menuName = "Overlord-Project/Narrative-Generator/Other/NarrativeFiles")]
    public class NarrativeFilesSO : ScriptableObject
    {
        [SerializeField] private List<string> narrativeFolders;

        public List<string> NarrativeFolders
        {
            get => narrativeFolders;
            set => narrativeFolders = value;
        }
    }
}

