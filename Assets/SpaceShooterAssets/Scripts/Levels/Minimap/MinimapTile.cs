using UnityEngine;
using UnityEngine.UI;
using Overlord.LevelGenerator.LevelSOs;

public class MinimapTile : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image currentOverlay;

    public RectTransform Rect => (RectTransform)transform;

    public void Init(DungeonRoomData data, MinimapIcons icons)
    {
        icon.sprite = icons.GetIcon(data.Type);
        CheckIfHasNPCs(data, icons);    // this only has meaning in the space shooter project
        currentOverlay.enabled = false;
    }

    private void CheckIfHasNPCs(DungeonRoomData data, MinimapIcons icons)
    {
        if (data.NumOfNpcs > 0)
            icon.sprite = icons.GetIcon("NPC");
    }

    public void SetVisible(bool visible)
    {
        icon.enabled = visible;
    }

    public void SetCurrent(bool current)
    {
        currentOverlay.enabled = current;
    }
}
