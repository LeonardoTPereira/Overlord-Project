using System;
using System.Collections.Generic;
using Game.Events;
using Game.GameManager;
using ScriptableObjects;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Game.LevelManager
{
    public class RoomBhv : MonoBehaviour
    {

        public static event StartRoomEvent StartRoomEventHandler;

        public DungeonRoom roomData;
        public List<int> northDoor;
        public List<int> southDoor;
        public List<int> eastDoor;
        public List<int> westDoor;

        public bool hasEnemies;
        private bool _hasPlacedInCenter;
        public Dictionary<EnemySO, int> enemiesDictionary;
        private int enemiesDead;
        private Vector3 position;

        public DoorBhv doorNorth;
        public DoorBhv doorSouth;
        public DoorBhv doorEast;
        public DoorBhv doorWest;

        public KeyBhv keyPrefab;
        public TriforceBhv triPrefab;
        public TreasureController treasurePrefab;
        public NpcController[] npcPrefabs;

        public Collider2D colNorth;
        public Collider2D colSouth;
        public Collider2D colEast;
        public Collider2D colWest;

        public TileBhv tilePrefab;
        public BlockBhv blockPrefab;

        public Sprite northWall, southWall, eastWall, westWall;
        public GameObject NWCollumn, NECollumn, SECollumn, SWCollumn;

        public GameObject minimapIcon;

        public List<Vector3> spawnPoints;

        protected Vector3 availablePosition;

        public static event EnterRoomEvent EnterRoomEventHandler;

        /// If true, the room is an Arena and do not have neighbors.
        private bool isArena = false;

        private void Awake()
        {
            hasEnemies = false;
            _hasPlacedInCenter = false;
            enemiesDictionary = new Dictionary<EnemySO, int>();
            enemiesDead = 0;
            isArena = GameManagerSingleton.Instance.arenaMode;
        }

        // Use this for initialization
        void Start()
        {
            // If the Arena Mode is on, then set up the Arena
            if (roomData.IsStartRoom() && isArena)
            {
                roomData.TotalEnemies = GameManagerSingleton.Instance.enemyLoader.arena.Length;
                hasEnemies = true;
            }

            SetLayout();
            position = transform.position;
            if (RoomHasKey())
            {
                PlaceKeysInRoom();
            }
            if (RoomHasTreasure())
            {
                PlaceTreasuresInRoom();
            }
            if (RoomHasNpc())
            {
                PlaceNpcsInRoom();
            }
            if (roomData.IsStartRoom())
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
                minimapIcon.GetComponent<SpriteRenderer>().color = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
                GetAvailablePosition();
                StartRoomEventHandler?.Invoke(this, new StartRoomEventArgs(availablePosition));
            }
            else if (roomData.IsFinalRoom())
            {
                PlaceTriforceInRoom();
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (GameManagerSingleton.Instance.enemyMode)
            {
                SelectEnemies();
            }

            minimapIcon.transform.localScale = new Vector3(roomData.Dimensions.Width, roomData.Dimensions.Height, 1);

            // If the Arena Mode is on, then spawn enemies in the Arena
            if (roomData.IsStartRoom() && isArena)
            {
                SpawnEnemies();
            }
        }

        private void DebugRoomData()
        {
            Debug.Log($"The current room: X {roomData.Coordinates.X}, Y {roomData.Coordinates.Y} has the keys with ");
            foreach (int keyID in roomData.KeyIDs)
            {
                Debug.Log($"Key ID: {keyID}");
            }
        }

        private void SetLayout()
        {
            SetKeysToDoors();

            var centerX = roomData.Dimensions.Width / 2.0f - 0.5f;
            var centerY = roomData.Dimensions.Height / 2.0f - 0.5f;
            const float delta = 0.0f; //para que os colisores das portas e das paredes não se sobreponham completamente
            //Posiciona as portas - são somados/subtraídos 1 para que as portas e colisores estejam periféricos à sala
            SetDoorsTransform(centerX, centerY, delta);

            SetCollidersOnRoom(centerX, centerY);

            InstantiateTiles(centerX, centerY);

            //Instantiate corner props
            GameObject auxObj;
            auxObj = Instantiate(NWCollumn, transform, true);
            auxObj.transform.localPosition = new Vector2(-1f - centerX, roomData.Dimensions.Height - centerY);
            auxObj = Instantiate(SECollumn, transform, true);
            auxObj.transform.localPosition = new Vector2(roomData.Dimensions.Width - centerX, -1f - centerY);
            auxObj = Instantiate(NECollumn, transform, true);
            auxObj.transform.localPosition = new Vector2(roomData.Dimensions.Width - centerX, roomData.Dimensions.Height - centerY);
            auxObj = Instantiate(SWCollumn, transform, true);
            auxObj.transform.localPosition = new Vector2(-1f - centerX, -1f - centerY);
        
            SetEnemySpawners(centerX, centerY);
        }

        private void InstantiateTiles(float centerX, float centerY)
        {
            GameObject auxObj;
            //Posiciona os tiles
            for (var ix = 0; ix < roomData.Dimensions.Width; ix++)
            {
                for (var iy = 0; iy < roomData.Dimensions.Height; iy++)
                {
                    var tileID = roomData.Tiles[ix, iy];
                    TileBhv tileObj;
                    if (tileID == (int) Enums.TileTypes.Block)
                    {
                        tileObj = Instantiate(blockPrefab);
                        if (ix == 0)
                        {
                            if (iy == 0)
                            {
                                auxObj = Instantiate(NWCollumn, transform, true);
                                auxObj.transform.localPosition =
                                    new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
                            }

                            if (iy == (roomData.Dimensions.Height - 1))
                            {
                                auxObj = Instantiate(SWCollumn, transform, true);
                                auxObj.transform.localPosition =
                                    new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
                            }

                            tileObj.GetComponent<SpriteRenderer>().sprite = westWall;
                        }
                        else if (iy == 0)
                        {
                            tileObj.GetComponent<SpriteRenderer>().sprite = northWall;
                        }
                        else if (ix == (roomData.Dimensions.Width - 1))
                        {
                            if (iy == (roomData.Dimensions.Height - 1))
                            {
                                auxObj = Instantiate(SECollumn, transform, true);
                                auxObj.transform.localPosition =
                                    new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
                            }

                            tileObj.GetComponent<SpriteRenderer>().sprite = eastWall;
                        }
                        else if (iy == (roomData.Dimensions.Height - 1))
                        {
                            tileObj.GetComponent<SpriteRenderer>().sprite = southWall;
                        }
                    }
                    else
                    {
                        tileObj = Instantiate(tilePrefab);
                    }

                    tileObj.SetPosition(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY, transform);
                }
            }
        }

        private void SetEnemySpawners(float centerX, float centerY)
        {
            var position = transform.position;
            var xOffset = position.x;
            var yOffset = position.y;

            var lowerHalfVer = (roomData.Dimensions.Height / Constants.nSpawnPointsHor);
            var upperHalfVer = (3 * roomData.Dimensions.Height / Constants.nSpawnPointsHor);
            var lowerHalfHor = (roomData.Dimensions.Width / Constants.nSpawnPointsVer);
            var upperHalfHor = (3 * roomData.Dimensions.Width / Constants.nSpawnPointsVer);
            var topHor = (Constants.distFromBorder +
                          (roomData.Dimensions.Width * (Constants.nSpawnPointsVer - 1) / Constants.nSpawnPointsVer));
            var topVer = (Constants.distFromBorder +
                          (roomData.Dimensions.Height * (Constants.nSpawnPointsHor - 1) / Constants.nSpawnPointsHor));

            //Create spawn points avoiding the points close to doors.
            for (var ix = Constants.distFromBorder;
                 ix < (roomData.Dimensions.Width - Constants.distFromBorder);
                 ix += (roomData.Dimensions.Width / Constants.nSpawnPointsVer))
            {
                for (var iy = Constants.distFromBorder;
                     iy < (roomData.Dimensions.Height - Constants.distFromBorder);
                     iy += (roomData.Dimensions.Height / Constants.nSpawnPointsHor))
                {
                    if (roomData.Tiles[ix, iy] == (int) Enums.TileTypes.Block) continue;
                    // Calculate the spawn point 2D position (spx, spy)
                    var spx = ix - centerX + xOffset;
                    var rh = roomData.Dimensions.Height - 1;
                    var spy = rh - iy - centerY + yOffset;
                    var point = new Vector3(spx, spy, 0);

                    // If the room is an Arena, then ignore the room center
                    if (isArena && Math.Abs(spx - 0.5f) < 0.001f && spy == 0.0f)
                    {
                        continue;
                    }

                    // Add the calculated point to spawn point list
                    if (ix <= Constants.distFromBorder || ix >= topHor)
                    {
                        if (iy >= lowerHalfVer && iy <= upperHalfVer) continue;
                        if (isArena) continue;
                        spawnPoints.Add(point);
                    }
                    else if (iy <= Constants.distFromBorder || iy >= topVer)
                    {
                        if (ix >= lowerHalfHor && ix <= upperHalfHor) continue;
                        if (isArena) continue;
                        spawnPoints.Add(point);
                    }
                    else
                    {
                        spawnPoints.Add(point);
                    }
                }
            }
        }

        private void SetCollidersOnRoom(float centerX, float centerY)
        {
            //Posiciona os colisores das paredes da sala
            colNorth.transform.localPosition = new Vector2(0.0f, centerY + 1);
            colSouth.transform.localPosition = new Vector2(0.0f, -centerY - 1);
            colEast.transform.localPosition = new Vector2(centerX + 1, 0.0f);
            colWest.transform.localPosition = new Vector2(-centerX - 1, 0.0f);
            colNorth.GetComponent<BoxCollider2D>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
            colSouth.GetComponent<BoxCollider2D>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
            colEast.GetComponent<BoxCollider2D>().size = new Vector2(1, roomData.Dimensions.Height + 2);
            colWest.GetComponent<BoxCollider2D>().size = new Vector2(1, roomData.Dimensions.Height + 2);

            //Ajusta sprites das paredes
            colNorth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
            colSouth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
            colEast.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(1, roomData.Dimensions.Height + 2);
            colWest.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(1, roomData.Dimensions.Height + 2);
        }

        private void SetDoorsTransform(float centerX, float centerY, float delta)
        {
            doorNorth.transform.localPosition = new Vector2(0.0f, centerY + 1 - delta);
            doorSouth.transform.localPosition = new Vector2(0.0f, -centerY - 1 + delta);
            doorEast.transform.localPosition = new Vector2(centerX + 1 - delta, 0.0f);
            doorWest.transform.localPosition = new Vector2(-centerX - 1 + delta, 0.0f);
        }

        private void OnDrawGizmos()
        {
            foreach (Vector3 spawnPoint in spawnPoints)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(spawnPoint, 1);
            }
        }

        private void SelectEnemies()
        {
            if (roomData.EnemiesByType == null && !isArena) return;
            if (roomData.EnemiesByType.EnemiesByTypeDictionary.Count == 0) return;
            hasEnemies = true;
            if (isArena)
            {
                // Load all the enemies from the folder
                EnemySO[] arena = GameManagerSingleton.Instance.enemyLoader.arena;
                foreach (var enemy in arena)
                {
                    enemiesDictionary.Add(enemy, 1);
                }
            }
            else
            {
                enemiesDictionary = EnemyDispenser.GetEnemiesForRoom(this);
            }
        }

        private void SpawnEnemies()
        {
            var selectedSpawnPoints = new List<int>();
            foreach (var enemiesFromType in enemiesDictionary)
            {
                for (var i = 0; i < enemiesFromType.Value; i++)
                {
                    int actualSpawn;
                    if (selectedSpawnPoints.Count >= spawnPoints.Count)
                    {
                        selectedSpawnPoints.Clear();
                    }
                    do
                    {
                        actualSpawn = Random.Range(0, spawnPoints.Count);
                    } while (selectedSpawnPoints.Contains(actualSpawn));
                    var enemy = GameManagerSingleton.Instance.enemyLoader.InstantiateEnemyFromScriptableObject(
                        new Vector3(spawnPoints[actualSpawn].x, spawnPoints[actualSpawn].y, 0f), 
                        transform.rotation, enemiesFromType.Key);
                    enemy.GetComponent<EnemyController>().SetRoom(this);
                    selectedSpawnPoints.Add(actualSpawn);
                }
            }
        }


        public void OnRoomEnter()
        {
            if (hasEnemies)
            {
                SpawnEnemies();
            }
            minimapIcon.GetComponent<SpriteRenderer>().color = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
            EnterRoomEventHandler?.Invoke(this, new EnterRoomEventArgs(roomData.Coordinates, hasEnemies, enemiesDictionary, -1, gameObject.transform.position, roomData.Dimensions));
        }

        public void CheckIfAllEnemiesDead()
        {
            enemiesDead++;
            if(enemiesDead == roomData.TotalEnemies)
            {
                hasEnemies = false;
                doorEast.OpenDoorAfterKilling();
                doorWest.OpenDoorAfterKilling();
                doorNorth.OpenDoorAfterKilling();
                doorSouth.OpenDoorAfterKilling();
            }
        }

        private void SetKeysToDoors()
        {
            doorNorth.keyID = northDoor;
            doorSouth.keyID = southDoor;
            doorEast.keyID = eastDoor;
            doorWest.keyID = westDoor;
        }

        private bool RoomHasKey()
        {
            return roomData.KeyIDs.Count > 0;
        }
        private void PlaceKeysInRoom()
        {
            foreach (int actualKey in roomData.KeyIDs)
            {
                PlaceKeyInRoom(actualKey);
            }
        }

        private void PlaceKeyInRoom(int keyId)
        {
            GetAvailablePosition();
            KeyBhv key = Instantiate(keyPrefab, availablePosition, transform.rotation);
            key.transform.position = availablePosition;
            key.KeyID = keyId;
        }

        private bool RoomHasTreasure()
        {
            return roomData.Items != null;
        }

        private void PlaceTreasuresInRoom()
        {
            foreach (var itemAmountPair in roomData.Items.ItemAmountBySo)
            {
                PlaceTreasureInRoom(itemAmountPair.Key, itemAmountPair.Value);
            }
        }
        private void PlaceTreasureInRoom(ItemSo item, int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                GetAvailablePosition();
                var treasure = Instantiate(treasurePrefab, transform);
                treasure.Treasure = item;
                treasure.transform.position = availablePosition;
            }
        }

        private void PlaceTriforceInRoom()
        {
            GetAvailablePosition();
            TriforceBhv tri = Instantiate(triPrefab, transform);
            tri.transform.position = availablePosition;
        
        }

        private void GetAvailablePosition()
        {
            if (!_hasPlacedInCenter)
            {
                availablePosition = roomData.GetCenterMostFreeTilePosition() + position;
                _hasPlacedInCenter = true;
            }
            else
            {
                availablePosition = roomData.GetNextAvailablePosition(availablePosition - position) + position;
            }
        }

        private bool RoomHasNpc(){
            return roomData.Npcs != null;
        }
    
        private void PlaceNpcsInRoom()
        {
            foreach (var npc in roomData.Npcs)
            {
                PlaceNpcInRoom(npc);
            }
        }

        private void PlaceNpcInRoom(NpcSO npc)
        {
            NpcController prefab = null;
            foreach (var npcPrefab in npcPrefabs)
            {
                if (npcPrefab.NpcSo == npc)
                    prefab = npcPrefab;
            }
            var npcController = Instantiate(prefab, transform);
            GetAvailablePosition();
            npcController.transform.position = availablePosition;
        }
    
    
    }
}
