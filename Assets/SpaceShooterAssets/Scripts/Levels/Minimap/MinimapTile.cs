using Overlord.LevelGenerator.LevelSOs;
using Overlord.NarrativeGenerator;
using Overlord.NarrativeGenerator.Quests;
using UnityEngine;
using UnityEngine.UI;

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
        QuestLineList _questlineList = FindObjectOfType<QuestGeneratorManager>().questLines;
        bool roomHasNpcInQuestList = false;
        if (_questlineList != null && _questlineList.NpcSos != null)
        {
            var matchingNpc = _questlineList.NpcSos.Find(npc =>
                npc != null &&
                npc.RoomCoordinates.X == data.Coordinates.X &&
                npc.RoomCoordinates.Y == data.Coordinates.Y);

            roomHasNpcInQuestList = matchingNpc != null;
        }

        if (data.NumOfNpcs > 0 && roomHasNpcInQuestList)
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
