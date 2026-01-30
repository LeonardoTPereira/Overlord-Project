using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NarrativeFilesRuntimeSetSO", menuName = "Overlord-Project/Narrative-Generator/Other/NarrativeFilesSet")]
    public class NarrativeFilesRuntimeSetSO : RuntimeSetSO<NarrativeFilesSO>
    {
        public NarrativeFilesSO GetNarrativesFromProfile(string profile)
        {
            foreach (var narrativeProfiles in Items)
            {
                if (narrativeProfiles.name.Equals(profile))
                    return narrativeProfiles;
            }
            return null;
        }
    }
}

