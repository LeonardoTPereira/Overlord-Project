using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.LevelManager
{
    public class DoorBHV : MonoBehaviour
    {

        //public GameManager gm;
        public List<int> keyID;
        public bool isOpen;
        public bool isClosedByEnemies;
        public Sprite lockedSprite;
        public Sprite closedSprite;
        public Sprite openedSprite;
        public Transform teleportTransform;
        public Material gradientMaterial;
        //	public int moveX;
        //	public int moveY;
        [SerializeField]
        private DoorBHV destination;
        private RoomBHV parentRoom;
        [SerializeField]
        private AudioClip unlockSnd;

        private AudioSource audioSrc;

        public static event ExitRoomEvent ExitRoomEventHandler;
        public static event KeyUsedEvent KeyUsedEventHandler;

        private void Awake()
        {
            isOpen = false;
            parentRoom = transform.parent.GetComponent<RoomBHV>();
            audioSrc = GetComponent<AudioSource>();
        }

        // Use this for initialization
        void Start()
        {
            int firstKeyID = -1;
            if (keyID != null)
            {
                if (keyID.Count > 0)
                    firstKeyID = keyID[0];
            }
            else
            {
                Debug.Log("Door shouldn't exist");
                Destroy(gameObject);
                return;
            }
            if (firstKeyID > 0)
            {

                Debug.Log("There is a key for this door, lock it");
                //Render the locked door sprite with the color relative to its ID
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                sr.sprite = lockedSprite;
                sr.material = gradientMaterial;
                sr.material.SetColor("gradientColor1", Util.colorId[firstKeyID - 1]);
                if (keyID.Count > 1)
                    sr.material.SetColor("gradientColor2", Util.colorId[keyID[1] - 1]);
                else
                    sr.material.SetColor("gradientColor2", Util.colorId[firstKeyID - 1]);
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

        void OnTriggerEnter2D(Collider2D other)
        {

            if (other.tag == "Player")
            {
                List<int> commonKeys = keyID.Intersect(Player.instance.keys).ToList();
                if (keyID.Count == 0 || isOpen)
                {
                    if (!isClosedByEnemies)
                    {
                        audioSrc.PlayOneShot(audioSrc.clip, 0.8f);
                        MovePlayerToNextRoom();
                    }
                }
                else if (commonKeys.Count == keyID.Count)
                {
                    if (!isClosedByEnemies)
                    {
                        audioSrc.PlayOneShot(unlockSnd, 0.7f);
                        foreach (int key in commonKeys)
                        {
                            if (!Player.instance.usedKeys.Contains(key))
                                Player.instance.usedKeys.Add(key);
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
            }
        }

        private void MovePlayerToNextRoom()
        {
            OnRoomExit(Player.instance.GetComponent<PlayerController>().GetHealth());
            //Enemy spawning logic here TODO make it better and work with the variable enemies SOs
            //Legacy: this was used when the doors were not closed when there are enemies in the room
            /*
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                parentRoom.hasEnemies = false;
            }
            else
            {
                foreach (GameObject enemy in enemies)
                {
                    Destroy(enemy);
                }
            }*/
            //The normal room transition
            Player.instance.transform.position = destination.teleportTransform.position;
            RoomBHV parent = destination.parentRoom;
            Coordinates cameraCoordinates = 
                new Coordinates(destination.parentRoom.roomData.Coordinates.X, destination.parentRoom.roomData.Coordinates.Y);
            Player.instance.AdjustCamera(cameraCoordinates, parent.roomData.Dimensions.Width);
            destination.transform.parent.GetComponent<RoomBHV>().OnRoomEnter();

        }

        public void SetDestination(DoorBHV other)
        {
            destination = other;
        }

        private void OnRoomExit(int playerHealth)
        {
            ExitRoomEventHandler?.Invoke(this, 
                new ExitRoomEventArgs(destination.parentRoom.roomData.Coordinates, playerHealth));
        }

        private void OnKeyUsed(int id)
        {
            KeyUsedEventHandler?.Invoke(this, new KeyUsedEventArgs(id));
        }

        public void OpenDoor()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sprite = openedSprite;
        }

        public void OpenDoorAfterKilling()
        {
            if ((keyID?.Count ?? -1) == 0 || isOpen)
            {
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                sr.sprite = openedSprite;
            }
            isClosedByEnemies = false;
        }
    }
}