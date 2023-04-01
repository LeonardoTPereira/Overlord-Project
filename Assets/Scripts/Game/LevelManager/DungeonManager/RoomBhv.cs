using System.Collections.Generic;
using System.Linq;
using Game.Audio;
using Game.EnemyManager;
using Game.Events;
using Game.GameManager;
using Game.LevelManager.DungeonLoader;
using Game.NarrativeGenerator.Quests;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using Util;
using Game.Quests;
using UnityEngine.Tilemaps;

namespace Game.LevelManager.DungeonManager
{
    public class RoomBhv : MonoBehaviour, ISoundEmitter, IQuestElement
    {

        public static event StartRoomEvent StartRoomEventHandler;
        public static event ShowRoomOnMiniMapEvent ShowRoomOnMiniMapEventHandler;

        protected Enums.RoomThemeEnum _theme;
        public DungeonRoom roomData;
        public List<int> northDoor;
        public List<int> southDoor;
        public List<int> eastDoor;
        public List<int> westDoor;

        public bool hasEnemies;
        public EnemyByAmountDictionary enemiesDictionary;
        private Vector3 _position;
        private Transform _transform;

        public DoorBhv doorNorth;
        public DoorBhv doorSouth;
        public DoorBhv doorEast;
        public DoorBhv doorWest;

        public GameObject keyPrefab;
        public GameObject triPrefab;
        public GameObject treasurePrefab;
        public GameObject readableItemPrefab;
        public GameObject[] npcPrefabs;

        public Collider2D colNorth;
        public Collider2D colSouth;
        public Collider2D colEast;
        public Collider2D colWest;

        public Tilemap floorTilemap;
        public Tilemap blockTilemap;
        public List<TileBase> blockTiles;
        public List<TileBase> floorTiles;
        protected TileBase _blockTile;
        protected TileBase _floorTile;

        protected Sprite _northWall;
        protected Sprite _southWall;
        protected Sprite _eastWall; 
        protected Sprite _westWall;
        public List<Sprite> northWalls;
        public List<Sprite> southWalls;
        public List<Sprite> eastWalls;
        public List<Sprite> westWalls;
        public List<GameObject> NWColumns;
        public List<GameObject> NEColumns;
        public List<GameObject> SEColumns;
        public List<GameObject> SWColumns;
        
        protected GameObject _nwColumn;
        protected GameObject _neColumn;
        protected GameObject _seColumn;
        protected GameObject _swColumn;

        public GameObject minimapIcon;

        public List<Vector3> spawnPoints;

        protected Vector3 _availablePosition;

        private EnemyLoader _enemyLoader;

        private List<GameObject> _instantiatedEnemies;
        private List<GameObject> _instantiatedKeys;
        

        private bool _hasBeenVisited;
        
        public static event EnterRoomEvent EnterRoomEventHandler;

        public int QuestId { get; set; }
        
        private void Awake()
        {
            hasEnemies = false;
            enemiesDictionary = new EnemyByAmountDictionary();
            _instantiatedEnemies = new List<GameObject>();
            _instantiatedKeys = new List<GameObject>();
            _hasBeenVisited = false;
            floorTilemap.ClearAllTiles();
        }

        // Use this for initialization
        private void Start()
        {
            _enemyLoader = GetComponent<EnemyLoader>();
        }

        private void DebugRoomData()
        {
            Debug.Log($"The current room: X {roomData.Coordinates.X}, Y {roomData.Coordinates.Y} has the keys with ");
            foreach (var keyID in roomData.KeyIDs)
            {
                Debug.Log($"Key ID: {keyID}");
            }
        }

        private void SetLayout()
        {
            SetKeysToDoors();

            SetDoorsTransform();

            SetCollidersOnRoom();

            InstantiateTileMap();

            InstantiateCornerProps();
        
            SetEnemySpawners();
        }

        protected virtual void InstantiateCornerProps()
        {
            var nwColumnObject =  Instantiate(_nwColumn, transform, true);
            nwColumnObject.transform.localPosition = new Vector2(-0.5f, roomData.Dimensions.Height+0.5f);
            var seColumnObject = Instantiate(_seColumn, transform, true);
            seColumnObject.transform.localPosition = new Vector2(roomData.Dimensions.Width+0.5f, -0.5f);
            var neColumnObject  = Instantiate(_neColumn, transform, true);
            neColumnObject.transform.localPosition = new Vector2(roomData.Dimensions.Width+0.5f, roomData.Dimensions.Height+0.5f);
            var swColumnObject = Instantiate(_swColumn, transform, true);
            swColumnObject.transform.localPosition = new Vector2(-0.5f, -0.5f);
        }

        protected virtual void InstantiateTileMap()
        {
	        for (var ix = 0; ix < roomData.Dimensions.Width; ix++)
	        {
		        for (var iy = 0; iy < roomData.Dimensions.Height; iy++)
		        {
			        var tileID = roomData.Tiles[ix, iy].TileType;
			        if (tileID == Enums.TileTypes.Block)
			        {
				        blockTilemap.SetTile(new Vector3Int(ix, iy), _blockTile);
			        }
			        else
			        {
				        floorTilemap.SetTile(new Vector3Int(ix, iy), _floorTile);
			        }
		        }
	        }
        }

        protected virtual  void SetEnemySpawners()
        {
            var roomPosition = transform.position;
            var xOffset = roomPosition.x;
            var yOffset = roomPosition.y;

            var lowerHalfVer = (roomData.Dimensions.Height / Constants.NSpawnPointsHor);
            var upperHalfVer = (3 * roomData.Dimensions.Height / Constants.NSpawnPointsHor);
            var lowerHalfHor = (roomData.Dimensions.Width / Constants.NSpawnPointsVer);
            var upperHalfHor = (3 * roomData.Dimensions.Width / Constants.NSpawnPointsVer);
            var topHor = (Constants.DistFromBorder +
                          (roomData.Dimensions.Width * (Constants.NSpawnPointsVer - 1) / Constants.NSpawnPointsVer));
            var topVer = (Constants.DistFromBorder +
                          (roomData.Dimensions.Height * (Constants.NSpawnPointsHor - 1) / Constants.NSpawnPointsHor));

            //Create spawn points avoiding the points close to doors.
            for (var ix = Constants.DistFromBorder;
                 ix < (roomData.Dimensions.Width - Constants.DistFromBorder);
                 ix += (roomData.Dimensions.Width / Constants.NSpawnPointsVer))
            {
                for (var iy = Constants.DistFromBorder;
                     iy < (roomData.Dimensions.Height - Constants.DistFromBorder);
                     iy += (roomData.Dimensions.Height / Constants.NSpawnPointsHor))
                {
                    if (roomData.Tiles[ix, iy].TileType == Enums.TileTypes.Block) continue;
                    // Calculate the spawn point 2D position (spx, spy)
                    var spx = ix + xOffset + 0.5f;
                    var spy = iy + yOffset + 0.5f;
                    var point = new Vector3(spx, spy, 0);

                    // Add the calculated point to spawn point list
                    if (ix <= Constants.DistFromBorder || ix >= topHor)
                    {
                        if (iy >= lowerHalfVer && iy <= upperHalfVer) continue;
                        spawnPoints.Add(point);
                    }
                    else if (iy <= Constants.DistFromBorder || iy >= topVer)
                    {
                        if (ix >= lowerHalfHor && ix <= upperHalfHor) continue;
                        spawnPoints.Add(point);
                    }
                    else
                    {
                        spawnPoints.Add(point);
                    }
                }
            }
        }

        protected virtual void SetCollidersOnRoom()
        {
	        SetSpritesTheme();
	        colNorth.transform.localPosition = new Vector2(roomData.Dimensions.Width/2f, -0.5f);
	        colSouth.transform.localPosition = new Vector2(roomData.Dimensions.Width/2f, roomData.Dimensions.Height+0.5f);
	        colEast.transform.localPosition = new Vector2(roomData.Dimensions.Width+0.5f, roomData.Dimensions.Height/2f);
	        colWest.transform.localPosition = new Vector2(-0.5f, roomData.Dimensions.Height/2f);
	        colNorth.GetComponent<BoxCollider2D>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
	        colSouth.GetComponent<BoxCollider2D>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
	        colEast.GetComponent<BoxCollider2D>().size = new Vector2(1, roomData.Dimensions.Height + 2);
	        colWest.GetComponent<BoxCollider2D>().size = new Vector2(1, roomData.Dimensions.Height + 2);
	        colNorth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
	        colSouth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
	        colEast.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(1, roomData.Dimensions.Height + 2);
	        colWest.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(1, roomData.Dimensions.Height + 2);
        }

        protected virtual void SetSpritesToWalls()
        {
            colNorth.gameObject.GetComponent<SpriteRenderer>().sprite = _northWall;
            colSouth.gameObject.GetComponent<SpriteRenderer>().sprite = _southWall;
            colEast.gameObject.GetComponent<SpriteRenderer>().sprite = _eastWall;
            colWest.gameObject.GetComponent<SpriteRenderer>().sprite = _westWall;
        }

        private void SetDoorsTransform()
        {
            doorNorth.transform.localPosition = new Vector2(roomData.Dimensions.Width/2f, roomData.Dimensions.Height+0.5f);
            doorSouth.transform.localPosition = new Vector2(roomData.Dimensions.Width/2f, -0.5f);
            doorEast.transform.localPosition = new Vector2(roomData.Dimensions.Width+0.5f, roomData.Dimensions.Height/2f);
            doorWest.transform.localPosition = new Vector2(-0.5f, roomData.Dimensions.Height/2f);
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
            if (roomData.EnemiesByType == null) return;
            if (roomData.EnemiesByType.EnemiesByTypeDictionary.Count == 0) return;
            hasEnemies = true;
            enemiesDictionary = roomData.EnemiesByType.GetEnemiesForRoom();
        }

        public virtual void SpawnEnemies()
        {
            var selectedSpawnPoints = new List<int>();
            _instantiatedEnemies.Clear();
            foreach (var enemiesFromType in enemiesDictionary)
            {
                foreach (var questId in enemiesFromType.Value.QuestIds)
                {
                    int currentSpawn;
                    if (selectedSpawnPoints.Count >= spawnPoints.Count)
                    {
                        selectedSpawnPoints.Clear();
                    }
                    do
                    {
                        currentSpawn = RandomSingleton.GetInstance().Next(0, spawnPoints.Count);
                    } while (selectedSpawnPoints.Contains(currentSpawn));
                    var enemy = _enemyLoader.InstantiateEnemyFromScriptableObject(
                    
                    new Vector3(spawnPoints[currentSpawn].x, spawnPoints[currentSpawn].y, 0f), 
                    transform.rotation, enemiesFromType.Key, questId);
                    _instantiatedEnemies.Add(enemy);
                    
                    RemoveFromDictionaryWhenEnemyDied(enemy);
                    selectedSpawnPoints.Add(currentSpawn);
                }
            }
        }

        protected virtual void RemoveFromDictionaryWhenEnemyDied(GameObject enemy)
        {
            enemy.GetComponent<EnemyController>().EnemyKilledHandler += RemoveFromDictionary;
        }

        public void OnRoomEnter()
        {
            if (hasEnemies)
            {
                ((ISoundEmitter)this).OnSoundEmitted(this, new EmitPitchedSfxEventArgs(AudioManager.SfxTracks.DoorClose, 1));
                SpawnEnemies();
                if (doorEast != null)
                {
	                doorEast.CloseDoor();
                }
                if (doorWest != null)
                {
	                doorWest.CloseDoor();
                }
                if (doorNorth != null)
                {
	                doorNorth.CloseDoor();
                }
                if (doorSouth != null)
                {
	                doorSouth.CloseDoor();
                }
            }

            if (!_hasBeenVisited)
            {
	            foreach (var key in _instantiatedKeys)
	            {
		            key.GetComponent<KeyBhv>().ShowKeyMinimapIcon();
	            }
                minimapIcon.GetComponent<SpriteRenderer>().color = Constants.VisitedColor;
                _hasBeenVisited = true;
            }
            EnterRoomEventHandler?.Invoke(this, new EnterRoomEventArgs(roomData.Coordinates, roomData.Dimensions, enemiesDictionary, transform.position));
            ((IQuestElement) this).OnQuestTaskResolved(this, new QuestExploreRoomEventArgs( roomData.Coordinates, QuestId ));
        }

        private void SetKeysToDoors()
        {
	        doorNorth.SetKey(northDoor);
	        doorSouth.SetKey(southDoor);
	        doorEast.SetKey(eastDoor);
	        doorWest.SetKey(westDoor);
        }

        private bool RoomHasKey()
        {
            return roomData.KeyIDs.Count > 0;
        }
        private void PlaceKeysInRoom()
        {
            foreach (var actualKey in roomData.KeyIDs)
            {
                PlaceKeyInRoom(actualKey);
            }
        }

        private void PlaceKeyInRoom(int keyId)
        {
            GetAvailablePosition();
            var key = PlaceObjectInRoom(keyPrefab);
            var keyBhv = key.GetComponent<KeyBhv>();
            keyBhv.KeyID = keyId;
            _instantiatedKeys.Add(key);
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
        private void PlaceTreasureInRoom(ItemSo item, QuestIdList questIds)
        {
            foreach (var questId in questIds.QuestIds)
            {
                if ( item as ReadableItemSo != null )
                    PlaceReadableItem(item, questId);
                else
                    PlaceTreasureItem(item, questId);
            }
        }

        private void PlaceReadableItem(ItemSo item, int questId)
        {
            GetAvailablePosition();
            var readableItem = PlaceObjectInRoom(readableItemPrefab);
            var readableItemController = readableItem.GetComponent<ReadableItemController>();
            readableItemController.SetItemInfo( item as ReadableItemSo, questId );
        }

        private void PlaceTreasureItem(ItemSo item, int questId)
        {
            GetAvailablePosition();
            var treasure = PlaceObjectInRoom(treasurePrefab);
            var treasureController = treasure.GetComponent<TreasureController>();
            treasureController.Treasure = item;
            treasureController.QuestId = questId;
        }

        private void PlaceTriforceInRoom()
        {
            GetAvailablePosition();
            PlaceObjectInRoom(triPrefab);
        }

        protected virtual void GetAvailablePosition()
        {
            _availablePosition = roomData.GetNextAvailablePosition();
        }

        protected virtual GameObject PlaceObjectInRoom(GameObject prefab)
        {
            var instance = Instantiate(prefab, transform, true);
            instance.transform.localPosition = _availablePosition;
            return instance;
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

        private void PlaceNpcInRoom(NpcSo npc)
        {
            GameObject prefab = null;
            foreach (var npcPrefab in npcPrefabs)
            {
                if (npcPrefab.GetComponent<NpcController>().Npc == npc)
                    prefab = npcPrefab;
            }
            GetAvailablePosition();
            PlaceObjectInRoom(prefab);
        }

        public void KillEnemies()
        {
            foreach (var enemy in _instantiatedEnemies.Where(enemy => enemy != null))
            {
                Destroy(enemy.gameObject);
            }
        }
        
        public void RemoveFromDictionary(object sender, EnemySO killedEnemyWeapon)
        {
            enemiesDictionary.Remove(killedEnemyWeapon);
            if (enemiesDictionary.Count != 0) return;
            hasEnemies = false;
            if (doorEast != null)
            {
	            doorEast.OpenDoorAfterKilling();
            }
            if (doorWest != null)
            {
	            doorWest.OpenDoorAfterKilling();
            }
            if (doorNorth != null)
            {
	            doorNorth.OpenDoorAfterKilling();
            }
            if (doorSouth != null)
            {
	            doorSouth.OpenDoorAfterKilling();
            }
        }

        public void MarkToVisit()
        {
            if (!_hasBeenVisited)
            {
                minimapIcon.GetComponent<SpriteRenderer>().color = Constants.MarkedColor;
            }
            ShowRoomOnMiniMapEventHandler?.Invoke(this, new ShowRoomOnMiniMapEventArgs(transform.position));
        }

        public void SetTheme(Enums.RoomThemeEnum theme)
        {
	        _theme = theme;
	        SetSpritesTheme();

	        SetLayout();
	        _transform = transform;
	        _position = _transform.position;
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
		        minimapIcon.GetComponent<SpriteRenderer>().color = Constants.VisitedColor;
		        GetAvailablePosition();
                CallStartRoomEvent();
		        _hasBeenVisited = true;
	        }
	        else if (roomData.IsFinalRoom())
	        {
		        PlaceTriforceInRoom();
		        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
	        }
	        SelectEnemies();

	        minimapIcon.transform.localScale = new Vector3(roomData.Dimensions.Width, roomData.Dimensions.Height, 1);
	        minimapIcon.transform.position += new Vector3(roomData.Dimensions.Width/2f, roomData.Dimensions.Height/2f, 0f);
        }

        protected virtual void CallStartRoomEvent()
        {
            StartRoomEventHandler?.Invoke(this, new StartRoomEventArgs(_availablePosition + _position));
        }

        protected virtual void SetSpritesTheme()
        {
            doorEast.SetTheme(_theme);
            doorWest.SetTheme(_theme);
            doorNorth.SetTheme(_theme);
            doorSouth.SetTheme(_theme);
            _nwColumn = NWColumns[(int) _theme];
            _neColumn = NEColumns[(int) _theme];
            _swColumn = SWColumns[(int) _theme];
            _seColumn = SEColumns[(int) _theme];
            _blockTile = blockTiles[(int) _theme];
            _floorTile = floorTiles[(int) _theme];
            _northWall = northWalls[(int) _theme];
            _southWall = southWalls[(int) _theme];
            _eastWall = eastWalls[(int) _theme];
            _westWall = westWalls[(int) _theme];
        }
    }
}
