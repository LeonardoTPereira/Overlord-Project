using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.DataCollection;
using Game.Events;
using Game.GameManager.Player;
using Game.LevelGenerator;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager;
using Game.Maestro;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;
using Random = System.Random;

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
#if UNITY_EDITOR
        [Foldout("Scriptable Objects"), Header("Enemy Components")]
#endif
        public EnemyComponentsSO enemyComponents;

        public QuestLine currentQuestLine;
        protected LevelGeneratorManager generator;
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
        private DungeonFileSo currentDungeonSO;
        private List<EnemySO> currentDungeonEnemies;

        public static GameManagerSingleton Instance { get; private set; }
        private Map map = null;
        public bool createMaps = false; //If true, runs the AE to create maps. If false, loads the ones on the designated folders
        public AudioSource audioSource;
        public AudioClip bgMusic, fanfarreMusic;
        public TextMeshProUGUI keyText, roomText, levelText;
        public List<RoomBhv> roomPrefabs;
        public Transform roomsParent;  //Transform to hold rooms for leaner hierarchy view
        public Dictionary<Coordinates, RoomBhv> roomBHVMap; //2D array for easy room indexing
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

        public bool IsLastQuestLine { get; set; }

        public static event EventHandler NewLevelLoadedEventHandler;
        public static event EventHandler GameStartEventHandler;
        public static event EnterRoomEvent EnterRoomEventHandler;
        public static event StartMapEvent StartMapEventHandler;
        public static event FinishMapEvent FinishMapEventHandler;

        public int maxTreasure, maxRooms, maxEnemies;
        public int mapFileMode;
        public GameObject panelIntro;

        /*Luana e Paolo*/
        public string levelFile;
        public bool arenaMode = false;
        public void Awake()
        {
            //Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            enemyLoader = gameObject.GetComponent<EnemyLoader>();
            audioSource = GetComponent<AudioSource>();
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
        private void Start()
        {
            GameStartEventHandler(null, EventArgs.Empty);
            //panelIntro.SetActive(true);   foi comentado
        }

        private void InstantiateRooms(RoomBhv roomBhv)
        {
            foreach (var currentPart in map.DungeonPartByCoordinates.Values.OfType<DungeonRoom>())
            {
                InstantiateRoom(currentPart, roomBhv);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            secondsElapsed += Time.deltaTime;
            if (!isInGame && generator != null && generator.hasFinished)
            {
                Instance.createdDungeon = generator.aux;
                if (startButton != null)
                    startButton.interactable = true;
            }
            if (isCompleted && generator != null && generator.hasFinished)
            {
                Instance.createdDungeon = generator.aux;
                currentMapId++;
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }

        }

        void LoadMap(DungeonFileSo dungeonFileSO)
        {
            map = new Map(dungeonFileSO, null, mapFileMode);
        }

        public Map GetMap()
        {
            return map;
        }

        private void InstantiateRoom(DungeonRoom dungeonRoom, RoomBhv roomPrefab)
        {
            var newRoom = Instantiate(roomPrefab, roomsParent);
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
            if (dungeonRoom.Items != null)
            {
                maxTreasure += dungeonRoom.Items.GetTotalItems();
            }
            //Sets room transform position
            newRoom.gameObject.transform.position = 
                new Vector2(roomSpacingX * dungeonRoom.Coordinates.X, -roomSpacingY * dungeonRoom.Coordinates.Y);
            roomBHVMap.Add(dungeonRoom.Coordinates, newRoom);
        }

        public void CreateConnectionsBetweenRooms(RoomBhv currentRoom)
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
            foreach (RoomBhv currentRoom in roomBHVMap.Values)
            {
                CreateConnectionsBetweenRooms(currentRoom);
            }
        }

        public void LoadNewLevel(DungeonFileSo dungeonFileSo)
        {
            ChangeMusic(bgMusic);
            maxTreasure = 0;
            //Loads map from data
            if (createMaps)
            {
                throw new NotImplementedException("Need to implement method to pass DungeonSO");
            }

            LoadMap(dungeonFileSo);
            
            EnemyDispenser.DistributeEnemiesInDungeon(map, currentQuestLine);
            ItemDispenser.DistributeItemsInDungeon(map, currentQuestLine);
            NpcDispenser.DistributeNpcsInDungeon(map, currentQuestLine);
            
            roomBHVMap = new Dictionary<Coordinates, RoomBhv>();

            Instance.enemyLoader.LoadEnemies(currentQuestLine.EnemySos);

            var selectedRoom = roomPrefabs[RandomSingleton.GetInstance().Random.Next(roomPrefabs.Count)];
            InstantiateRooms(selectedRoom);
            ConnectRoooms();
            OnStartMap(dungeonFileSo.name, currentTestBatchId, map);
        }

        private void OnStartMap(string mapName, int batch, Map map)
        {
            int totalEnemies = roomBHVMap[map.StartRoomCoordinates].enemiesDictionary.Sum(x => x.Value);
            StartMapEventHandler?.Invoke(this, new StartMapEventArgs(mapName, batch, map, projectileSet.Items.IndexOf(projectileType), maxTreasure));
            EnterRoomEventHandler?.Invoke(this, new EnterRoomEventArgs(map.StartRoomCoordinates, roomBHVMap[map.StartRoomCoordinates].hasEnemies, null
                , -1, roomBHVMap[map.StartRoomCoordinates].gameObject.transform.position, (map.DungeonPartByCoordinates[map.StartRoomCoordinates] as DungeonRoom).Dimensions));
        }

        private void OnApplicationQuit()
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
        }

        private void EndGame(object sender, EventArgs eventArgs)
        {
            FinishMapEventHandler?.Invoke(this, new FinishMapEventArgs(map));
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            LevelLoaderBHV.loadLevelButtonEventHandler += SetCurrentLevelSO;
            LevelLoaderBHV.loadLevelButtonEventHandler += SetCurrentLevelQuestLine;
            WeaponLoaderBHV.LoadWeaponButtonEventHandler += SetProjectileSO;
            DungeonLoader.LoadLevelEventHandler += SetCurrentLevelSO;
            DungeonLoader.LoadLevelEventHandler += SetCurrentLevelQuestLine;
            PlayerController.PlayerDeathEventHandler += GameOver;
            TriforceBhv.GotTriforceEventHandler += LevelComplete;
            FormBHV.PostTestFormQuestionAnsweredEventHandler += EndGame;
            DungeonLoader.LoadLevelEventHandler += CheckIfLastAvailableQuestline;
        }
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
            LevelLoaderBHV.loadLevelButtonEventHandler -= SetCurrentLevelSO;
            LevelLoaderBHV.loadLevelButtonEventHandler -= SetCurrentLevelQuestLine;
            WeaponLoaderBHV.LoadWeaponButtonEventHandler -= SetProjectileSO;
            DungeonLoader.LoadLevelEventHandler -= SetCurrentLevelSO;
            DungeonLoader.LoadLevelEventHandler -= SetCurrentLevelQuestLine;
            PlayerController.PlayerDeathEventHandler -= GameOver;
            TriforceBhv.GotTriforceEventHandler -= LevelComplete;
            FormBHV.PostTestFormQuestionAnsweredEventHandler -= EndGame;
            DungeonLoader.LoadLevelEventHandler -= CheckIfLastAvailableQuestline;
        }

        private void SetProjectileSO(object sender, LoadWeaponButtonEventArgs eventArgs)
        {
            projectileType = eventArgs.ProjectileSO;
        }
        
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
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

        public void StopMusic()
        {
            audioSource.Stop();
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
            currentDungeonSO = args.DungeonFileSo;
        }
        
        public void SetCurrentLevelQuestLine(object sender, LevelLoadEventArgs args)
        {
            currentQuestLine = args.LevelQuestLine;
        }
        
        public void CheckIfLastAvailableQuestline(object sender, LevelLoadEventArgs args)
        {
            IsLastQuestLine = args.IsLastQuestLine;
        }
        public void OnLevelLoadedEvents()
        {
            NewLevelLoadedEventHandler?.Invoke(null, EventArgs.Empty);
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

        private void SetDestinations(Coordinates targetCoordinates, Coordinates sourceCoordinates, int orientation)
        {
            switch (orientation)
            {
                case 1:
                    roomBHVMap[sourceCoordinates].doorWest.SetDestination(roomBHVMap[targetCoordinates].doorEast);
                    roomBHVMap[targetCoordinates].doorEast.SetDestination(roomBHVMap[sourceCoordinates].doorWest);
                    break;
                case 2:
                    roomBHVMap[sourceCoordinates].doorNorth.SetDestination(roomBHVMap[targetCoordinates].doorSouth);
                    roomBHVMap[targetCoordinates].doorSouth.SetDestination(roomBHVMap[sourceCoordinates].doorNorth);
                    break;
            }
        }
    }
}

