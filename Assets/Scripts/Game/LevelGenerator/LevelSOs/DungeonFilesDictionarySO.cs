using UnityEngine;

namespace Game.LevelGenerator.LevelSOs
{
    [CreateAssetMenu(menuName = "DungeonFiles/DungeonFilesDictionarySO")]
    public class DungeonFilesDictionarySO : ScriptableObject
    {
        public ProfileDungeonDictionary dungeonsForProfile = new ProfileDungeonDictionary();
        public void Add(string profile, DungeonFileSo dungeonSO)
        {
            if (!dungeonsForProfile.ContainsKey(profile))
            {
                dungeonsForProfile.Add(profile, new DungeonFileSOList());
            }
            dungeonsForProfile[profile].dungeonFileSOList.Add(dungeonSO);
        }

        public void Remove(string profile)
        {
            if (dungeonsForProfile.ContainsKey(profile))
                dungeonsForProfile.Remove(profile);
        }
    }
}