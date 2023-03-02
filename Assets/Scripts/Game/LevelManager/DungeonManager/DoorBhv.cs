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
        public List<Sprite> closedSprites;
        public List<Sprite> openedSprites;
        private Sprite _closedSprite;
        private Sprite _openedSprite;
        public Transform teleportTransform;
        public Material gradientMaterial;
        [SerializeField]
        private DoorBhv destination;

        private RoomBhv _currentRoom;        
        private Color _color;
        private SpriteRenderer _doorSprite;
        private static readonly int GradientColor1 = Shader.PropertyToID("gradientColor1");
        private static readonly int GradientColor2 = Shader.PropertyToID("gradientColor2");

        public static event ExitRoomEvent ExitRoomEventHandler;
        public static event KeyUsedEvent KeyUsedEventHandler;

        private void Awake()
        {
            isOpen = false;
            _currentRoom = transform.parent.GetComponent<RoomBhv>();
            _doorSprite = GetComponent<SpriteRenderer>();
        }

        private bool DestroyIfDoesNotExist()
        {
	        if (keyID != null) return false;
	        Destroy(gameObject);
	        return true;
        }

        private void SetSprite()
        {
	        var firstKeyID = GetFirstKeyId();
	        if (_currentRoom.hasEnemies && isClosedByEnemies)
	        {
		        if (firstKeyID > 0)
		        {
			        SetLockedSprite(firstKeyID);
		        }
		        else
		        {
			        _doorSprite.sprite = _closedSprite;
		        }
	        }
	        else
	        {
		        if (firstKeyID > 0 && !isOpen)
		        {
			        SetLockedSprite(firstKeyID);
		        }
		        else
		        {
			        _doorSprite.sprite = _openedSprite;
		        }
	        }
        }

        private int GetFirstKeyId()
        {
            if (keyID.Count > 0)
                return keyID[0];
            return -1;
        }

        private void SetLockedSprite(int firstKeyID)
        {
            _doorSprite.sprite = lockedSprite;
            _doorSprite.material = gradientMaterial;
            _doorSprite.material.SetColor(GradientColor1, Constants.ColorId[firstKeyID - 1]);
            _doorSprite.material.SetColor(GradientColor2,
                keyID.Count > 1 ? Constants.ColorId[keyID[1] - 1] : Constants.ColorId[firstKeyID - 1]);
            _color = Constants.ColorId[firstKeyID - 1];
        }

        private void OnDrawGizmos()
        {
            if (keyID.Count > 0)
            {
                Gizmos.color = _color;
                Gizmos.DrawSphere(transform.position, 4);
            }
            else
            {
                DrawGizmoCorridors();
            }
        }

        private void DrawGizmoCorridors()
        {
            var offsetX = 0;
            var offsetY = 0;
            switch (gameObject.name)
            {
                case "Door North":
                    offsetY = 4;
                    break;
                case "Door South":
                    offsetY = -4;
                    break;
                case "Door East":
                    offsetX = 4;
                    break;
                case "Door West":
                    offsetX = -4;
                    break;
            }
            Gizmos.color = Color.black;
            Gizmos.DrawCube(transform.position + new Vector3(offsetX, offsetY, 0),
                new Vector3(4 + Mathf.Abs(offsetX), 4 + Mathf.Abs(offsetY), 1));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("PlayerTrigger")) return;
            
            
            var commonKeys = keyID.Intersect(DungeonPlayer.Instance.Keys).ToList();
            if (keyID.Count == 0 || isOpen)
            {
                if (_currentRoom.hasEnemies && isClosedByEnemies) return;
                MovePlayerToNextRoom();
            }
            
            else if (commonKeys.Count == keyID.Count)
            {
                if (_currentRoom.hasEnemies && isClosedByEnemies) return;
                UseKeys(commonKeys);
                MovePlayerToNextRoom();
            }

        }

        private void UseKeys(List<int> commonKeys)
        {
            ((ISoundEmitter) this).OnSoundEmitted(this, new EmitPitchedSfxEventArgs(AudioManager.SfxTracks.LockOpen, 1));
            foreach (var key in commonKeys.Where(key => !DungeonPlayer.Instance.UsedKeys.Contains(key)))
            {
                DungeonPlayer.Instance.UsedKeys.Add(key);
            }

            OpenDoor();
            destination.OpenDoor();
            isOpen = true;
            destination.isOpen = true;
            OnKeyUsed(commonKeys.First());
        }

        private void MovePlayerToNextRoom()
        {
            _currentRoom.KillEnemies();
            ExitRoomEventHandler?.Invoke(this, new ExitRoomEventArgs(destination._currentRoom.roomData.Coordinates, -1, destination.teleportTransform.position));
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

        private void OpenDoor()
        {
            _doorSprite.sprite = _openedSprite;
        }

        public void OpenDoorAfterKilling()
        {
            if ((keyID?.Count ?? -1) == 0 || isOpen)
            {
                _doorSprite.sprite = _openedSprite;
            }
            isClosedByEnemies = false;
        }

        public void SetTheme(Enums.RoomThemeEnum theme)
        {
	        _closedSprite = closedSprites[(int) theme];
	        _openedSprite = openedSprites[(int) theme];
        }

        public void CloseDoor()
        {
	        if (DestroyIfDoesNotExist()) return;
	        SetSprite();
        }

        public void SetKey(List<int> keyIDs)
        {
	        keyID = keyIDs;
	        if (DestroyIfDoesNotExist()) return;
	        SetSprite();
        }
    }
}