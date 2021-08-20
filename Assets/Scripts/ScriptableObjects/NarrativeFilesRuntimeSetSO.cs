using UnityEngine;

[CreateAssetMenu]
public class NarrativeFilesRuntimeSetSO : EnemyGenerator.RuntimeSetSO<NarrativeFilesSO>
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
