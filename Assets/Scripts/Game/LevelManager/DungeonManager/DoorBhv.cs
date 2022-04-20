using System.Collections.Generic;
using System.Linq;
using Game.Audio;
using Game.Events;
using Game.GameManager.Player;
using UnityEngine;
using Util;

namespace Game.LevelManager.DungeonManager
{
    public class DoorBhv : MonoBehaviour, ISoundEmitter
    {
        public List<int> keyID;
        public bool isOpen;
        public bool isClosedByEnemies;
        public Sprite lockedSprite;
        public Sprite closedSprite;
        public Sprite openedSprite;
        public Transform teleportTransform;
        public Material gradientMaterial;
        [SerializeField]
        private DoorBhv destination;

        private RoomBhv parentRoom;        
        private Color color;
        private SpriteRenderer sr;

        public static event ExitRoomEvent ExitRoomEventHandler;
        public static event KeyUsedEvent KeyUsedEventHandler;

        private void Awake()
        {
            isOpen = false;
            parentRoom = transform.parent.GetComponent<RoomBhv>();
        }

        // Use this for initialization
        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            int firstKeyID = -1;
            if (keyID != null)
            {
                if (keyID.Count > 0)
                    firstKeyID = keyID[0];
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            if (firstKeyID > 0)
            {
                //Render the locked door sprite with the color relative to its ID
                var sr = GetComponent<SpriteRenderer>();
                sr.sprite = lockedSprite;
                sr.material = gradientMaterial;
                sr.material.SetColor("gradientColor1", Constants.colorId[firstKeyID - 1]);
                if (keyID.Count > 1)
                    sr.material.SetColor("gradientColor2", Constants.colorId[keyID[1] - 1]);
                else
                    sr.material.SetColor("gradientColor2", Constants.colorId[firstKeyID - 1]);
                color = Constants.colorId[firstKeyID - 1];
            }
            if (parentRoom.hasEnemies)
            {
                isClosedByEnemies = true;
                if (keyID.Count == 0 || isOpen)
                {
                    SpriteRenderer sr = GetComponent<SpriteRenderer>();
                    sr.sprite = closedSprite;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (keyID.Count > 0)
            {
                Gizmos.color = color;
                Gizmos.DrawSphere(transform.position, 4);
            }
            else
            {
                DrawGizmoCorridors();
            }
        }

        private void DrawGizmoCorridors()
        {
            int offsetX = 0;
            int offsetY = 0;
            if (gameObject.name.Equals("Door North"))
            {
                offsetY = 4;
            }
            else if (gameObject.name.Equals("Door South"))
            {
                offsetY = -4;
            }
            else if (gameObject.name.Equals("Door East"))
            {
                offsetX = 4;
            }
            else if (gameObject.name.Equals("Door West"))
            {
                offsetX = -4;
            }
            Gizmos.color = Color.black;
            Gizmos.DrawCube(transform.position + new Vector3(offsetX, offsetY, 0),
                new Vector3(4 + Mathf.Abs(offsetX), 4 + Mathf.Abs(offsetY), 1));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("PlayerTrigger")) return;
            var commonKeys = keyID.Intersect(Player.Instance.keys).ToList();
            if (keyID.Count == 0 || isOpen)
            {
                if (isClosedByEnemies) return;
                MovePlayerToNextRoom();
            }
            else if (commonKeys.Count == keyID.Count)
            {
                if (isClosedByEnemies) return;
                ((ISoundEmitter)this).OnSoundEmitted(this, new EmitPitchedSfxEventArgs(AudioManager.SfxTracks.LockOpen, 1));
                foreach (var key in commonKeys.Where(key => !Player.Instance.usedKeys.Contains(key)))
                {
                    Player.Instance.usedKeys.Add(key);
                }
                OpenDoor();
                if (!destination.parentRoom.hasEnemies)
                    destination.OpenDoor();
                isOpen = true;
                destination.isOpen = true;
                OnKeyUsed(commonKeys.First());
                MovePlayerToNextRoom();
            }
        }

        private void MovePlayerToNextRoom()
        {
            ExitRoomEventHandler?.Invoke(this, new ExitRoomEventArgs(destination.parentRoom.roomData.Coordinates, -1, destination.teleportTransform.position));
            destination.transform.parent.GetComponent<RoomBhv>().OnRoomEnter();
        }

        public void SetDestination(DoorBhv other)
        {
            destination = other;
        }

        private void OnKeyUsed(int id)
        {
            KeyUsedEventHandler?.Invoke(this, new KeyUsedEventArgs(id));
        }

        public void OpenDoor()
        {
            sr.sprite = openedSprite;
        }

        public void OpenDoorAfterKilling()
        {
            if ((keyID?.Count ?? -1) == 0 || isOpen)
            {
                sr.sprite = openedSprite;
            }
            isClosedByEnemies = false;
        }
    }
}