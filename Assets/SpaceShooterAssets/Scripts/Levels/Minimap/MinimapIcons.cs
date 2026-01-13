using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SpaceShooter/UI/MiniMapIcons")]
public class MinimapIcons : ScriptableObject
{
    [System.Serializable]
    public class IconEntry
    {
        public string roomType;
        public Sprite icon;
    }

    [SerializeField] private List<IconEntry> icons;

    public Sprite GetIcon(string type)
    {
        foreach (var entry in icons)
            if (entry.roomType == type)
                return entry.icon;

        return null;
    }
}
