using Overlord.LevelGenerator.LevelSOs;
using UnityEngine;

public class RoomExitArrow : MonoBehaviour
{
    private DungeonRoomData from;
    private DungeonRoomData to;

    private bool locked;

    public void Init(DungeonRoomData fromRoom, DungeonRoomData toRoom)
    {
        from = fromRoom;
        to = toRoom;

        locked = to.Locks != null && !DungeonRuntimeData.HasKey(Mathf.Abs(to.Locks[0]));
        UpdateVisual();
    }

    void UpdateVisual()
    {
        // ativa/desativa X acima da seta
        transform.Find("LockIcon")?.gameObject.SetActive(locked);
    }

    void Update()
    {
        if (locked) return;

        if (Input.GetKeyDown(GetDirectionKey()) &&
            Input.GetKeyDown(GetDirectionKey())) // double tap simples
        {
            LevelLoader.Instance.MoveToRoom(to);
        }
    }

    KeyCode GetDirectionKey()
    {
        Vector2 delta = new(
            to.Coordinates.X - from.Coordinates.X,
            to.Coordinates.Y - from.Coordinates.Y
        );

        if (delta == Vector2.up) return KeyCode.UpArrow;
        if (delta == Vector2.down) return KeyCode.DownArrow;
        if (delta == Vector2.left) return KeyCode.LeftArrow;
        return KeyCode.RightArrow;
    }
}
