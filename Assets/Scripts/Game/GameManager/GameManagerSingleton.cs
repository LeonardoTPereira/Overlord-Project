using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game.LevelManager;
using LevelGenerator;
using MyBox;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.GameManager
{
    public class GameManagerSingleton : MonoBehaviour
    {
#if UNITY_EDITOR
        [Foldout("Scriptable Objects"), Header("Set With All Possible Treasures")]
#endif
        public TreasureRuntimeSetSO treasureSet;
#if UNITY_EDITOR
        [Foldout("Scriptable Objects"), Header("Set With All Possible Projectiles")]
#endif
        public ProjectileTypeRuntimeSetSO projectileSet;
#if UNITY_EDITOR
        [Foldout("Scriptable Objects"), Header("Current Projectile")]
#endif
        public ProjectileTypeSO projectileType;
        protected Program generator;
        public Dungeon createdDungeon;
#if UNITY_EDITOR
        [Separator("Other Stuff")]
#endif
        [SerializeField]
        Button startButton;
        [SerializeField]
        TextMeshProUGUI progressText;
        private IEnumerator coroutine;
        private bool isCompleted;
        private bool isInGame;
        [SerializeField]
        private LevelConfigRuntimeSetSO levelSet;
        private DungeonFileSO currentDungeonSO;

        public static GameManagerSingleton instance = null;
        protected TextAsset mapFile;
        private List<TextAsset> rooms = new List<TextAsset>();
        private List<int> randomLevelList = new List<int>();
        private Map map = null;
        public bool createMaps = false; //If true, runs the AE to create maps. If false, loads the ones on the designated folders
        public AudioSource audioSource;
        public AudioClip bgMusic, fanfarreMusic;
        public TextMeshProUGUI keyText, roomText, levelText;
        public RoomBHV roomPrefab;
        public Transform roomsParent;  //Transform to hold rooms for leaner hierarchy view
        public Dictionary<Coordinates, RoomBHV> roomBHVMap; //2D array for easy room indexing
        public float roomSpacingX = 30f; //Spacing between rooms: X
        public float roomSpacingY = 20f; //Spacing between rooms: Y
        private string mapDirectory;
        private int currentMapId = 0;
        private int currentTestBatchId = 0;
        public bool readRooms = true;
        public GameObject gameOverScreen, victoryScreen, introScreen, gameUI;

        private static float secondsElapsed = 0;

        public bool createEnemy, survivalMode, enemyMode;
        public EnemyLoader enemyLoader;
        public HealthUI healthUI;
        public KeyUI keyUI;

        public int nExecutions, nBatches;

        public static event EventHandler NewLevelLoadedEventHandler;
        public static event EventHandler GameStartEventHandler;
        public static event EnterRoomEvent EnterRoomEventHandler;
        public static event StartMapEvent StartMapEventHandler;
        public static event FinishMapEvent FinishMapEventHandler;

        public int maxTreasure, maxRooms;
        public int mapFileMode;
        public GameObject panelIntro;

        /*Luana e Paolo*/
        public string levelFile;

        public void Awake()
        {
            //Singleton
            if (instance == null)
            {
                instance = this;

                enemyLoader = gameObject.GetComponent<EnemyLoader>();
                audioSource = GetComponent<AudioSource>();

                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

        }

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory, string search, ref string[] files)
        {
            // Process the list of files found in the directory.
            files = Directory.GetFiles(targetDirectory, search);
            foreach (string file in files)
            {
                TextAsset asset = Resources.Load<TextAsset>("test");
            }
        }

        // Use this for initialization
        void Start()
        {
            GameStartEventHandler(null, EventArgs.Empty);
            //panelIntro.SetActive(true);   foi comentado
        }

        void InstantiateRooms()
        {
            foreach (DungeonPart currentPart in map.DungeonPartByCoordinates.Values)
            {
                if (currentPart is DungeonRoom)
                {
                    InstantiateRoom(currentPart as DungeonRoom);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            secondsElapsed += Time.deltaTime;
            if (!isInGame && generator != null && generator.hasFinished)
            {
                instance.createdDungeon = generator.aux;
                if (startButton != null)
                    startButton.interactable = true;
            }
            if (isCompleted && generator != null && generator.hasFinished)
            {
                instance.createdDungeon = generator.aux;
                currentMapId++;
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }

        }

        void LoadMap(DungeonFileSO dungeonFileSO)
        {
            map = new Map(dungeonFileSO, null, mapFileMode);
        }

        public Map GetMap()
        {
            return map;
        }

        void InstantiateRoom(DungeonRoom dungeonRoom)
        {
            RoomBHV newRoom = Instantiate(roomPrefab, roomsParent);
            newRoom.roomData = dungeonRoom;
            Coordinates targetCoordinates;
            newRoom.westDoor = null;
            newRoom.eastDoor = null;
            newRoom.northDoor = null;
            newRoom.southDoor = null;
            if (IsLeftEdge(dungeonRoom.Coordinates))
            { // west
                targetCoordinates = new Coordinates(dungeonRoom.Coordinates.X - 1, dungeonRoom.Coordinates.Y);
                newRoom.westDoor = CheckCorridor(targetCoordinates);
            }
            if (IsBottomEdge(dungeonRoom.Coordinates))
            { // north
                targetCoordinates = new Coordinates(dungeonRoom.Coordinates.X, dungeonRoom.Coordinates.Y - 1);
                newRoom.northDoor = CheckCorridor(targetCoordinates);
            }
            if (IsRightEdge(dungeonRoom.Coordinates))
            {
                targetCoordinates = new Coordinates(dungeonRoom.Coordinates.X + 1, dungeonRoom.Coordinates.Y);
                newRoom.eastDoor = CheckCorridor(targetCoordinates);
            }
            if (IsTopEdge(dungeonRoom.Coordinates))
            {
                targetCoordinates = new Coordinates(dungeonRoom.Coordinates.X, dungeonRoom.Coordinates.Y + 1);
                newRoom.southDoor = CheckCorridor(targetCoordinates);
            }
            if (dungeonRoom.Treasure > 0)
            {
                maxTreasure += treasureSet.Items[dungeonRoom.Treasure-1].value;
            }
            //Sets room transform position
            newRoom.gameObject.transform.position = 
                new Vector2(roomSpacingX * dungeonRoom.Coordinates.X, -roomSpacingY * dungeonRoom.Coordinates.Y);
            roomBHVMap.Add(dungeonRoom.Coordinates, newRoom);
        }

        public void CreateConnectionsBetweenRooms(RoomBHV currentRoom)
        {
            Coordinates targetCoordinates;
            if (currentRoom.westDoor != null)
            { // west
                targetCoordinates = new Coordinates(currentRoom.roomData.Coordinates.X - 2, currentRoom.roomData.Coordinates.Y);
                SetDestinations(targetCoordinates, currentRoom.roomData.Coordinates, 1);
            }
            if (currentRoom.northDoor != null)
            { // west
                targetCoordinates = new Coordinates(currentRoom.roomData.Coordinates.X, currentRoom.roomData.Coordinates.Y - 2);
                SetDestinations(targetCoordinates, currentRoom.roomData.Coordinates, 2);
            }
        }

        public void ConnectRoooms()
        {
            foreach (RoomBHV currentRoom in roomBHVMap.Values)
            {
                CreateConnectionsBetweenRooms(currentRoom);
            }
        }

        public void LoadNewLevel(DungeonFileSO dungeonFileSO)
        {
            ChangeMusic(bgMusic);
            //Loads map from data
            if (createMaps)
            {
                map = new Map(instance.createdDungeon);
                mapFile = null;
            }
            else
            {
                LoadMap(dungeonFileSO);
            }

            roomBHVMap = new Dictionary<Coordinates, RoomBHV>();

            InstantiateRooms();
            ConnectRoooms();
            OnStartMap(dungeonFileSO.name, currentTestBatchId, map);
        }

        private void OnStartMap(string mapName, int batch, Map map)
        {
            Debug.Log("Starting Map");
            StartMapEventHandler(this, new StartMapEventArgs(mapName, batch, map, projectileSet.Items.IndexOf(projectileType)));
            EnterRoomEventHandler(this, new EnterRoomEventArgs(map.StartRoomCoordinates, roomBHVMap[map.StartRoomCoordinates].hasEnemies, 
                roomBHVMap[map.StartRoomCoordinates].enemiesIndex, -1, roomBHVMap[map.StartRoomCoordinates].gameObject.transform.position, (map.DungeonPartByCoordinates[map.StartRoomCoordinates] as DungeonRoom).Dimensions));
        }

        void OnApplicationQuit()
        {
            AnalyticsEvent.GameOver();
        }

        private void LevelComplete(object sender, EventArgs eventArgs)
        {
            ChangeMusic(fanfarreMusic);
            gameUI.SetActive(false);
            //TODO save every gameplay data
            //TODO make it load a new level

            //Analytics for the level
            if (!createMaps && !survivalMode)
                victoryScreen.SetActive(true);
            else
                CheckEndOfBatch();

        }

        public void CheckEndOfBatch()
        {
            FinishMapEventHandler(this, new FinishMapEventArgs(map));
            isCompleted = true;
        }

        private void EndGame(object sender, EventArgs eventArgs)
        {
            FinishMapEventHandler(this, new FinishMapEventArgs(map));
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            LevelLoaderBHV.loadLevelButtonEventHandler += SetCurrentLevelSO;
            DungeonTester.loadLevelEventHandler += SetCurrentLevelSO;
            WeaponLoaderBHV.LoadWeaponButtonEventHandler += SetProjectileSO;
            PostFormMenuBHV.postFormButtonEventHandler += SetCurrentLevelSO;
            DungeonEntrance.loadLevelEventHandler += SetCurrentLevelSO;
            PlayerController.PlayerDeathEventHandler += GameOver;
            TriforceBHV.GotTriforceEventHandler += LevelComplete;
            FormBHV.PostTestFormAnswered += EndGame;
        }
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
            LevelLoaderBHV.loadLevelButtonEventHandler -= SetCurrentLevelSO;
            DungeonTester.loadLevelEventHandler -= SetCurrentLevelSO;
            WeaponLoaderBHV.LoadWeaponButtonEventHandler -= SetProjectileSO;
            PostFormMenuBHV.postFormButtonEventHandler -= SetCurrentLevelSO;
            DungeonEntrance.loadLevelEventHandler -= SetCurrentLevelSO;
            PlayerController.PlayerDeathEventHandler -= GameOver;
            TriforceBHV.GotTriforceEventHandler -= LevelComplete;
            FormBHV.PostTestFormAnswered -= EndGame;
        }

        void SetProjectileSO(object sender, LoadWeaponButtonEventArgs eventArgs)
        {
            projectileType = eventArgs.ProjectileSO;
        }



        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Level" || scene.name == "LevelWithEnemies")
            {
                isInGame = true;
                startButton = null;
                isCompleted = false;

                gameUI.SetActive(true);
                healthUI = gameUI.GetComponentInChildren<HealthUI>();
                keyUI = gameUI.GetComponentInChildren<KeyUI>();
                OnLevelLoadedEvents();
                LoadNewLevel(currentDungeonSO);
            }
            if (scene.name == "Main")
            {
                introScreen.SetActive(true);
            }
        }

        public void ChangeMusic(AudioClip music)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            if (music == fanfarreMusic)
            {
                audioSource.volume = 0.3f;
            }
            else
                audioSource.volume = 0.7f;
            audioSource.clip = music;
            audioSource.loop = true;
            audioSource.Play();
        }

        //TODO display something about the player losing and call a continue screen os something like this.
        private void GameOver(object sender, EventArgs eventArgs)
        {
            gameUI.SetActive(false);
            gameOverScreen.SetActive(true);
        }

        public void MainMenu()
        {
            GameStartEventHandler(null, EventArgs.Empty);
            SceneManager.LoadScene("Main");
        }

        public void SetCurrentLevelSO(object sender, LevelLoadEventArgs args)
        {
            currentDungeonSO = args.DungeonFileSO;
        }

        public bool HasMoreLevels()
        {
            return false;
        }

        public void OnLevelLoadedEvents()
        {
            NewLevelLoadedEventHandler(null, EventArgs.Empty);
        }

        public bool IsLeftEdge(Coordinates coordinates)
        {
            return coordinates.X > 1;
        }

        public bool IsRightEdge(Coordinates coordinates)
        {
            return coordinates.X < (map.Dimensions.Width - 1);
        }

        public bool IsBottomEdge(Coordinates coordinates)
        {
            return coordinates.Y > 1;
        }
        public bool IsTopEdge(Coordinates coordinates)
        {
            return coordinates.Y < (map.Dimensions.Height - 1);
        }

        public List<int> CheckCorridor(Coordinates targetCoordinates)
        {
            if (map.DungeonPartByCoordinates.ContainsKey(targetCoordinates))
            {
                //Sets door
                DungeonLockedCorridor lockedCorridor = map.DungeonPartByCoordinates[targetCoordinates] as DungeonLockedCorridor;

                if (lockedCorridor != null)
                {
                    return lockedCorridor.LockIDs;
                }
                else
                {
                    return new List<int>();
                }
            }
            return null;
        }

        public void SetDestinations(Coordinates targetCoordinates, Coordinates sourceCoordinates, int orientation)
        {
            if (orientation == 1)
            {
                roomBHVMap[sourceCoordinates].doorWest.SetDestination(roomBHVMap[targetCoordinates].doorEast);
                roomBHVMap[targetCoordinates].doorEast.SetDestination(roomBHVMap[sourceCoordinates].doorWest);
            }
            else if (orientation == 2)
            {
                roomBHVMap[sourceCoordinates].doorNorth.SetDestination(roomBHVMap[targetCoordinates].doorSouth);
                roomBHVMap[targetCoordinates].doorSouth.SetDestination(roomBHVMap[sourceCoordinates].doorNorth);
            }
        }
    }
}

