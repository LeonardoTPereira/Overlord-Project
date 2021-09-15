using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "DungeonFiles/DungeonFilesDictionarySO")]
public class DungeonFilesDictionarySO : ScriptableObject
{
    public ProfileDungeonDictionary dungeonsForProfile = new ProfileDungeonDictionary();
    public void Add(string profile, DungeonFileSO dungeonSO)
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