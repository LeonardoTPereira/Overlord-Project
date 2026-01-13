using Overlord.LevelGenerator.LevelSOs;
using UnityEngine;

public class RoomExitArrow : MonoBehaviour
{
    private DungeonRoomData from;
    private DungeonRoomData corridorBetween;
    private DungeonRoomData to;

    private bool locked;

    private KeyCode directionKey;
    private float lastKeyTime = -1f;
    private const float doubleTapTime = 0.3f; // ajuste se quiser

    private System.Action _onExitRoom;

    public void Init(DungeonRoomData fromRoom,DungeonRoomData corridorBetweenRoom, DungeonRoomData toRoom, System.Action onExitRoom)
    {
        from = fromRoom;
        corridorBetween = corridorBetweenRoom;
        to = toRoom;
        _onExitRoom = onExitRoom;

        locked = to.Locks != null && !DungeonRuntimeData.HasKey(Mathf.Abs(to.Locks[0]));

        directionKey = GetDirectionKey();
        SetupTransform();
    }

    void Update()
    {
        if (locked) return;

        if (Input.GetKeyDown(directionKey))
        {
            if (Time.time - lastKeyTime <= doubleTapTime)
            {
                _onExitRoom?.Invoke();                  // Chama função para sair da sala (só deleta npc por enquanto)
                LevelLoader.Instance.MoveToRoom(to);
                lastKeyTime = -1f; // reseta
            }
            else
            {
                lastKeyTime = Time.time;
            }
        }
    }

    void SetupTransform()
    {
        Vector3 worldOffset;
        Quaternion worldRotation;

        Vector2 delta = new(
            corridorBetween.Coordinates.X - from.Coordinates.X,
            corridorBetween.Coordinates.Y - from.Coordinates.Y
        );

        if (delta == Vector2.right)
        {
            worldOffset = new Vector3(0.2f, 0f, 0f);
            worldRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (delta == Vector2.left)
        {
            worldOffset = new Vector3(-1.2f, 0f, 0f);
            worldRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (delta == Vector2.up)
        {
            worldOffset = new Vector3(-0.5f, 0.6f, 0f);
            worldRotation = Quaternion.Euler(0, 0, 90);
        }
        else // down
        {
            worldOffset = new Vector3(-0.5f, -0.6f, 0f);
            worldRotation = Quaternion.Euler(0, 0, 270);
        }

        Transform roomTransform = transform.parent;
        transform.position = roomTransform.position + worldOffset;
        transform.rotation = worldRotation;
    }

    KeyCode GetDirectionKey()
    {
        Vector2 delta = new(
            corridorBetween.Coordinates.X - from.Coordinates.X,
            corridorBetween.Coordinates.Y - from.Coordinates.Y
        );

        if (delta == Vector2.up) return KeyCode.UpArrow;
        if (delta == Vector2.down) return KeyCode.DownArrow;
        if (delta == Vector2.left) return KeyCode.LeftArrow;
        return KeyCode.RightArrow;
    }
}
