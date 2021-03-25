using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using TMPro;
using LevelGenerator;
using UnityEngine.UI;
using EnemyGenerator;
using System;

public class GameManager : MonoBehaviour
{
    public int nRooms, nKeys, nLocks;
    public float linearity;

    public TreasureRuntimeSetSO treasureSet;
    public ProjectileTypeRuntimeSetSO projectileSet;
    public ProjectileTypeSO projectileType;
    Program generator;
    public Dungeon createdDungeon;
    [SerializeField]
    Button startButton;
    [SerializeField]
    TextMeshProUGUI progressText;
    private IEnumerator coroutine;
    private bool isCompleted;
    private bool isInGame;
    [SerializeField]
    private LevelConfigRuntimeSetSO levelSet;
    private List<string> levelSetNames;
    private string currentLevel;

    public static GameManager instance = null;
    //private List<TextAsset> maps = new List<TextAsset>();
    protected TextAsset mapFile;
    private List<TextAsset> rooms = new List<TextAsset>();
    private List<int> randomLevelList = new List<int>();
    private Map map = null;
    public bool createMaps = true; //If true, runs the AE to create maps. If false, loads the ones on the designated folders
    public AudioSource audioSource;
    public AudioClip bgMusic, fanfarreMusic;
    public TextMeshProUGUI keyText, roomText, levelText;
    public RoomBHV roomPrefab;
    public Transform roomsParent;  //Transform to hold rooms for leaner hierarchy view
    public Dictionary<Coordinates, RoomBHV> roomBHVMap; //2D array for easy room indexing
    public float roomSpacingX = 30f; //Spacing between rooms: X
    public float roomSpacingY = 20f; //Spacing between rooms: Y
    private string mapDirectory;
    //private static string[] maps = null;
    //private static string[] rooms = null;
    private int currentMapId = 0;
    private int currentTestBatchId = 0;
    //public string mapFilePath = "Assets/Data/map.txt"; //Path to load map data from
    //public string roomsFilePath = "Assets/Data/rooms.txt";
    public bool readRooms = true;
    public GameObject gameOverScreen, victoryScreen, introScreen, gameUI;
    //public MainMenu mainMenu;

    public enum LevelPlayState { InProgress, Won, Lost, Skip, Quit }
    public static LevelPlayState state = LevelPlayState.InProgress;
    private static float secondsElapsed = 0;

    public bool createEnemy, survivalMode, enemyMode;
    public EnemyLoader enemyLoader;
    public int dungeonDifficulty, chosenDifficulty;
    public HealthUI healthUI;
    public KeyUI keyUI;

    public int nExecutions, nBatches;

    public delegate void NewLevelLoadedEvent();
    public static event NewLevelLoadedEvent newLevelLoadedEvent;

    public int maxTreasure, maxRooms;
    public int mapFileMode;
    public GameObject panelIntro;
    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;

            generator = new Program();
            enemyLoader = gameObject.GetComponent<EnemyLoader>();
            audioSource = GetComponent<AudioSource>();

            //TODO Apply selected difficulty in this
            dungeonDifficulty = 50;

            //readRooms = false;
            //createEnemy = false;
            DontDestroyOnLoad(gameObject);
            AnalyticsEvent.GameStart();
            //Debug.Log("Level Order");
            for (int i = 0; i < 6; ++i)
            {
                int aux;
                do
                {
                    aux = UnityEngine.Random.Range(0, 6);
                }
                while (randomLevelList.Contains(aux));
                randomLevelList.Add(aux);
                //Debug.Log(aux);
            }
            //Used for the level generator experiments
            /*maps.Add(Resources.Load<TextAsset>("Batch0/Lizard"));
            maps.Add(Resources.Load<TextAsset>("Batch0/MyMoon"));
            maps.Add(Resources.Load<TextAsset>("Batch0/MyLizard"));
            maps.Add(Resources.Load<TextAsset>("Batch0/Dragon"));
            maps.Add(Resources.Load<TextAsset>("Batch0/MyDragon"));
            maps.Add(Resources.Load<TextAsset>("Batch0/Moon"));
            maps.Add(Resources.Load<TextAsset>("Levels/Easy"));
            maps.Add(Resources.Load<TextAsset>("Levels/Medium"));
            maps.Add(Resources.Load<TextAsset>("Levels/Hard"));*/

            nExecutions = 0;
            nBatches = 0;
            levelSetNames = new List<string>();
            foreach (LevelConfigSO level in levelSet.Items)
            {
                levelSetNames.Add(level.fileName);
            }

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //mapDirectory = Application.dataPath + "/Data/Batch";



        /*if (Directory.Exists(mapDirectory + currentTestBatchId))
        {
            Debug.Log(mapDirectory + currentTestBatchId);
            // This path is a directory
            ProcessDirectory(mapDirectory + currentTestBatchId, "map*txt", ref maps);
            ProcessDirectory(mapDirectory + currentTestBatchId, "room*txt", ref rooms);
        }
        else
        {
            Debug.Log("Something is wrong with the map directory!");
        }*/


    }

    // Process all files in the directory passed in, recurse on any directories 
    // that are found, and process the files they contain.
    public static void ProcessDirectory(string targetDirectory, string search, ref string[] files)
    {
        // Process the list of files found in the directory.
        files = Directory.GetFiles(targetDirectory, search);
        foreach (string file in files)
        {
            //AssetDatabase.ImportAsset(file);
            TextAsset asset = Resources.Load<TextAsset>("test");
            //Debug.Log("File: " + file);
        }
    }

    // Use this for initialization
    void Start()
    {
        PlayerProfile.instance.OnGameStart();
        //LoadNewLevel();
        //panelIntro.SetActive(true);   foi comentado
    }

    void InstantiateRooms()
    {
        foreach (DungeonPart currentPart in map.dungeonPartByCoordinates.Values)
        {
            if(currentPart is DungeonRoom)
            {
                InstantiateRoom(currentPart as DungeonRoom);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        secondsElapsed += Time.deltaTime;
        if (!isInGame && generator != null)
        {
            if (generator.hasFinished)
            {
                instance.createdDungeon = generator.aux;
                if (startButton != null)
                    startButton.interactable = true;
            }
        }
        if (isCompleted && generator != null)
        {
            if (generator.hasFinished)
            {
                instance.createdDungeon = generator.aux;
                currentMapId++;
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

    }

    void LoadMap(TextAsset mapFile)
    {
        /*if (readRooms)
        { //deve ler também os tiles das salas?
            map = new Map(mapFile.text, rooms[mapId].text);
        }*/
        //else
        //{ //apenas as salas, sem tiles
        map = new Map(mapFile.text, null, mapFileMode);
        //}
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
        Debug.Log($"Now instantiating room: {dungeonRoom.coordinates}");
        newRoom.westDoor = null;
        newRoom.eastDoor = null;
        newRoom.northDoor = null;
        newRoom.southDoor = null;
        if (IsLeftEdge(dungeonRoom.coordinates))
        { // west
            targetCoordinates = new Coordinates(dungeonRoom.coordinates.X - 1, dungeonRoom.coordinates.Y);
            newRoom.westDoor = CheckCorridor(targetCoordinates);
        }
        if (IsBottomEdge(dungeonRoom.coordinates))
        { // north
            targetCoordinates = new Coordinates(dungeonRoom.coordinates.X, dungeonRoom.coordinates.Y - 1);
            newRoom.northDoor = CheckCorridor(targetCoordinates);
        }
        if (IsRightEdge(dungeonRoom.coordinates))
        {
            targetCoordinates = new Coordinates(dungeonRoom.coordinates.X + 1, dungeonRoom.coordinates.Y);
            newRoom.eastDoor = CheckCorridor(targetCoordinates);
        }
        if(IsTopEdge(dungeonRoom.coordinates))
        {
            targetCoordinates = new Coordinates(dungeonRoom.coordinates.X, dungeonRoom.coordinates.Y + 1);
            newRoom.southDoor = CheckCorridor(targetCoordinates);
        }
        if (dungeonRoom.Treasure > -1)
        {
#if UNITY_EDITOR
            Debug.Log($"We have a treasure with ID: {dungeonRoom.Treasure}");
#endif
            maxTreasure += treasureSet.Items[dungeonRoom.Treasure].value;
        }
        //Sets room transform position
        newRoom.gameObject.transform.position = new Vector2(roomSpacingX * dungeonRoom.coordinates.X, -roomSpacingY * dungeonRoom.coordinates.Y);
        roomBHVMap.Add(dungeonRoom.coordinates, newRoom);
    }

    public void CreateConnectionsBetweenRooms(RoomBHV currentRoom)
    {
        Coordinates targetCoordinates;
        if (currentRoom.westDoor != null)
        { // west
            targetCoordinates = new Coordinates(currentRoom.roomData.coordinates.X - 2, currentRoom.roomData.coordinates.Y);
            SetDestinations(targetCoordinates, currentRoom.roomData.coordinates, 1);
        }
        if (currentRoom.northDoor != null)
        { // west
            targetCoordinates = new Coordinates(currentRoom.roomData.coordinates.X, currentRoom.roomData.coordinates.Y - 2);
            SetDestinations(targetCoordinates, currentRoom.roomData.coordinates, 2);
        }
    }

    public void ConnectRoooms()
    {
        foreach (RoomBHV currentRoom in roomBHVMap.Values)
        {
            CreateConnectionsBetweenRooms(currentRoom);
        }
    }

    public void LoadNewLevel(TextAsset mapFile, int difficulty = 1)
    {
        switch (difficulty)
        {
            case 0:
                dungeonDifficulty = (int)(EnemyUtil.easyFitness * 3f);
                break;
            case 1:
                dungeonDifficulty = (int)(EnemyUtil.mediumFitness * 3f);
                break;
            case 2:
                dungeonDifficulty = (int)(EnemyUtil.hardFitness * 3f);
                break;
        }
        Time.timeScale = 1f;
        ChangeMusic(bgMusic);
        //Loads map from data
        if (createMaps)
        {
            map = new Map(instance.createdDungeon);
            mapFile = null;
        }
        else
            LoadMap(mapFile);
        //NEED TO CALL ANALYTICS?
        //AnalyticsEvent.LevelStart(randomLevelList[currentMapId]);

        roomBHVMap = new Dictionary<Coordinates, RoomBHV>();

        InstantiateRooms();
        ConnectRoooms();
        Player.instance.keys.Clear();
        Player.instance.usedKeys.Clear();
        Player.instance.AdjustCamera(map.startRoomCoordinates, (map.dungeonPartByCoordinates[map.startRoomCoordinates] as DungeonRoom).Dimensions.Width);
        

        UpdateLevelGUI();
        UpdateRoomGUI(map.startRoomCoordinates.X, map.startRoomCoordinates.Y);
        OnStartMap(mapFile.name, currentTestBatchId, map, difficulty);
    }

    private void OnStartMap(string mapName, int batch, Map map, int difficulty)
    {
        //Debug.Log("Map Name: " + mapName);
        PlayerProfile.instance.OnMapStart(mapName, batch, map, difficulty, projectileSet.Items.IndexOf(projectileType));
        PlayerProfile.instance.OnRoomEnter(map.startRoomCoordinates, roomBHVMap[map.startRoomCoordinates].hasEnemies, roomBHVMap[map.startRoomCoordinates].enemiesIndex, Player.instance.GetComponent<PlayerController>().GetHealth());
        //Debug.Log("Started Profiling");
    }

    void OnApplicationQuit()
    {
        AnalyticsEvent.GameOver();
    }

    public void SetLevelPlayState(LevelPlayState newState)
    {
        state = newState;
    }

    public void LevelComplete()
    {
        ChangeMusic(fanfarreMusic);
        Time.timeScale = 0f;
        gameUI.SetActive(false);
        //TODO save every gameplay data
        //TODO make it load a new level
        //Debug.Log("MapID:" + randomLevelList[currentMapId]);
        //Debug.Log("MapsLength:" + maps.Count);

        //Analytics for the level
        Dictionary<string, object> customParams = new Dictionary<string, object>();
        customParams.Add("seconds_played", secondsElapsed);
        customParams.Add("keys", Player.instance.keys.Count);
        customParams.Add("locks", Player.instance.usedKeys.Count);
        PlayerProfile.instance.HasFinished = true;
        if (!createMaps && !survivalMode)
            victoryScreen.SetActive(true);
        else
            CheckEndOfBatch();

        switch (state)
        {
            case LevelPlayState.Won:
                AnalyticsEvent.LevelComplete(currentTestBatchId + randomLevelList[currentMapId], customParams);
                break;
            case LevelPlayState.Lost:
                AnalyticsEvent.LevelFail(currentTestBatchId + randomLevelList[currentMapId], customParams);
                break;
            case LevelPlayState.Skip:
                AnalyticsEvent.LevelSkip(currentTestBatchId + randomLevelList[currentMapId], customParams);
                break;
            case LevelPlayState.InProgress:
            case LevelPlayState.Quit:
            default:
                AnalyticsEvent.LevelQuit(currentTestBatchId + randomLevelList[currentMapId], customParams);
                break;
        }

    }

    public void CheckEndOfBatch()
    {
        PlayerProfile.instance.OnMapComplete(map);
        /*if (!createMaps && survivalMode)
        {
            if (currentMapId < (maps.Count - 1))
            {
                Debug.Log("Next map");
                currentMapId++;
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
            else
            {
                Debug.Log("Load New Batch");
                LoadNewBatch();
            }
        }
        else
        {*/
            isCompleted = true;
        //}
    }

    public void EndGame()
    {
        PlayerProfile.instance.OnMapComplete(map);
        //endingScreen.SetActive(true);
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        LevelLoaderBHV.loadLevelButtonEvent += PlayGameOnDifficulty;
        WeaponLoaderBHV.loadWeaponButtonEvent += SetProjectileSO;
        PostFormMenuBHV.postFormButtonEvent += PlayGameOnDifficulty;
    }
    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        LevelLoaderBHV.loadLevelButtonEvent -= PlayGameOnDifficulty;
        WeaponLoaderBHV.loadWeaponButtonEvent -= SetProjectileSO;
        PostFormMenuBHV.postFormButtonEvent -= PlayGameOnDifficulty;
    }

    void SetProjectileSO(ProjectileTypeSO type)
    {
        projectileType = type;
    }



    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LevelGenerator")
        {
            if (createMaps && !survivalMode)
            {
                Constants.nV = nRooms;
                Constants.nK = nKeys;
                Constants.nL = nLocks;
                Constants.lCoef = linearity;
                //coroutine = generator.CreateDungeonParallel(progressText);
                //coroutine = generator.CreateDungeon();
                //StartCoroutine(coroutine);
                generator.CreateDungeon();
            }
            isInGame = false;
            startButton = GameObject.Find("StartButton").GetComponent<Button>();
            //Level Generator Stuff
            startButton.interactable = false;
        }

        if (scene.name == "Level" || scene.name == "LevelWithEnemies")
        {
            isInGame = true;
            startButton = null;
            isCompleted = false;

            if(enemyMode)
                enemyLoader.LoadEnemies(chosenDifficulty);

            Player pl = Player.instance;
            pl.cam = Camera.main;
            //Recover health
            pl.gameObject.GetComponent<PlayerController>().ResetHealth();

            gameUI.SetActive(true);
            healthUI = gameUI.GetComponentInChildren<HealthUI>();
            keyUI = gameUI.GetComponentInChildren<KeyUI>();
            OnLevelLoadedEvents();
            LoadNewLevel(mapFile, chosenDifficulty);
        }
        if (scene.name == "Main")
        {
            introScreen.SetActive(true);
        }
    }

    void OnDestroy()
    {

    }

    public void UpdateKeyGUI()
    {
        //keyText.text = "x" + Player.instance.keys.Count;
    }

    public void UpdateRoomGUI(int x, int y)
    {
        //roomText.text = "Sala: " + x/2 + "," + y/2;
    }

    public void UpdateLevelGUI()
    {
        //int aux = currentMapId + 1 + (currentTestBatchId * maps.Count);
        //healthUI.CreateHeartImage(); foi comentado
        //keyUI.CreateKeyImage(); foi comentado
        //levelText.text = "Nível: " + aux + "/12";
    }

    public void ChangeMusic(AudioClip music)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
        if (music == fanfarreMusic)
        {
            //Debug.Log("Decreasing volume");
            audioSource.volume = 0.3f;
        }
        else
            audioSource.volume = 0.7f;
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();
    }

    //TODO display something about the player losing and call a continue screen os something like this.
    public void GameOver()
    {
        Time.timeScale = 1f;
        gameUI.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void MainMenu()
    {
        PlayerProfile.instance.OnGameStart();
        SceneManager.LoadScene("Main");
    }

    public void SetLevelMode(string fileName, int difficulty)
    {
        chosenDifficulty = difficulty;
        //Debug.Log("Nome do Arquivo: " + fileName);
        mapFile = Resources.Load<TextAsset>("Levels/"+fileName);
        currentLevel = fileName;
        //Debug.Log("Mapa: " + mapFile);
        levelSetNames.Remove(fileName);
        //SceneManager.LoadScene("LevelWithEnemies");
    }

        

    public void Rezero()
    {
        /*if (nBatches < 9)
        {*/
        instance.createEnemy = false;

        /*if (nExecutions < 201)
        {
            GameManagerTest.instance.AwakeInit();
            GameManagerTest.instance.StartInit();
            nExecutions++;
        }*/
        /*else
        {
            switch (nBatches)
            {
                case 0:
                    GameManagerTest.instance.difficulty = GameManagerTest.DifficultyEnum.medium;
                    break;
                case 1:
                    GameManagerTest.instance.difficulty = GameManagerTest.DifficultyEnum.hard;
                    break;
                case 2:
                    GameManagerTest.instance.difficulty = GameManagerTest.DifficultyEnum.medium;
                    EnemyUtil.maxGenerations = 100;
                    break;
                case 3:
                    GameManagerTest.instance.difficulty = GameManagerTest.DifficultyEnum.medium;
                    EnemyUtil.maxGenerations = 10;
                    break;
                case 4:
                    GameManagerTest.instance.difficulty = GameManagerTest.DifficultyEnum.medium;
                    EnemyUtil.maxGenerations = 30;
                    EnemyUtil.popSize = 1000;
                    break;
                case 5:
                    GameManagerTest.instance.difficulty = GameManagerTest.DifficultyEnum.medium;
                    EnemyUtil.maxGenerations = 30;
                    EnemyUtil.popSize = 100000;
                    break;
                case 6:
                    GameManagerTest.instance.difficulty = GameManagerTest.DifficultyEnum.medium;
                    EnemyUtil.maxGenerations = 10;
                    EnemyUtil.popSize = 1000;
                    break;
                case 7:
                    GameManagerTest.instance.difficulty = GameManagerTest.DifficultyEnum.medium;
                    EnemyUtil.maxGenerations = 10;
                    EnemyUtil.popSize = 100;
                    break;
                case 8:
                    GameManagerTest.instance.difficulty = GameManagerTest.DifficultyEnum.easy;
                    EnemyUtil.maxGenerations = 30;
                    EnemyUtil.popSize = 10000;
                    break;
            }
            nExecutions = 0;
            nBatches++;
            GameManagerTest.instance.AwakeInit();
            GameManagerTest.instance.StartInit();
        }*/
        //}
    }

    public void PlayGameOnDifficulty(string fileName, int difficulty)
    {
        instance.SetLevelMode(fileName, difficulty);
    }

    public bool HasMoreLevels()
    {
        if (levelSetNames.Count > 0)
            return true;
        return false;
    }

    public LevelConfigSO PickNextLevel()
    {
        LevelConfigSO curLevel = null;
        string nextLevelCandidate = levelSetNames[0];
        bool hasFound = false;
        if (PlayerProfile.instance.HasFinished)
        {
            foreach (string levelName in levelSetNames)
            {
                curLevel = levelSet.Items.Find(x => (x.fileName.CompareTo(levelName) == 0));
                if (curLevel.enemy == chosenDifficulty)
                {
                    nextLevelCandidate = curLevel.fileName;
                    hasFound = true;
                    break;
                }
            }
            if(!hasFound)
            {
                foreach (string levelName in levelSetNames)
                {
                    curLevel = levelSet.Items.Find(x => (x.fileName.CompareTo(levelName) == 0));
                    if (Math.Abs(curLevel.enemy - chosenDifficulty) == 1)
                    {
                        nextLevelCandidate = curLevel.fileName;
                        hasFound = true;
                        break;
                    }
                }
            }
            if(!hasFound)
            {
                nextLevelCandidate = levelSetNames[0];
                hasFound = true;
            }
        }
        else
        {
            foreach (string levelName in levelSetNames)
            {
                curLevel = levelSet.Items.Find(x => (x.fileName.CompareTo(levelName) == 0));
                if ((chosenDifficulty - 1) == curLevel.enemy)
                {
                    nextLevelCandidate = curLevel.fileName;
                    hasFound = true;
                    break;
                }
            }
            if(!hasFound)
            {
                foreach (string levelName in levelSetNames)
                {
                    curLevel = levelSet.Items.Find(x => (x.fileName.CompareTo(levelName) == 0));
                    if ((chosenDifficulty - 2) == curLevel.enemy)
                    {
                        nextLevelCandidate = curLevel.fileName;
                        hasFound = true;
                        break;
                    }
                }
            }
            if (!hasFound)
            {
                nextLevelCandidate = levelSetNames[0];
                hasFound = true;
            }
        }

        levelSetNames.Remove(nextLevelCandidate);

        return levelSet.Items.Find(x => (x.fileName.CompareTo(nextLevelCandidate) == 0));
    }

    public void OnLevelLoadedEvents()
    {
        newLevelLoadedEvent();
    }

    public bool IsLeftEdge(Coordinates coordinates)
    {
        return coordinates.X > 1;
    }

    public bool IsRightEdge(Coordinates coordinates)
    {
        return coordinates.X < (map.dimensions.Width - 1);
    }

    public bool IsBottomEdge(Coordinates coordinates)
    {
        return coordinates.Y > 1;
    }
    public bool IsTopEdge(Coordinates coordinates)
    {
        return coordinates.Y < (map.dimensions.Height - 1);
    }

    public List<int> CheckCorridor(Coordinates targetCoordinates)
    {
        if (map.dungeonPartByCoordinates.ContainsKey(targetCoordinates))
        {
            //Sets door
            Debug.Log($"The door exists at {targetCoordinates}");
            DungeonLockedCorridor lockedCorridor = map.dungeonPartByCoordinates[targetCoordinates] as DungeonLockedCorridor;
            Debug.Log($"Is Locked Corridor? {lockedCorridor}");

            if (lockedCorridor != null)
            {
                Debug.Log($"There are {(lockedCorridor).lockIDs.Count} locks in this door");
                foreach (int locks in (lockedCorridor).lockIDs)
                {
                    Debug.Log($"Lock ID: {locks}");
                }
                return lockedCorridor.lockIDs;
            }
            else
            {
                Debug.Log("Is only a corridor");
                return new List<int>();
            }
        }
        else
        {
            Debug.Log($"Corridor not found {targetCoordinates}");
        }
        return null;
    }

    public void SetDestinations(Coordinates targetCoordinates, Coordinates sourceCoordinates, int orientation)
    {
        if(orientation == 1)
        {
            roomBHVMap[sourceCoordinates].doorWest.SetDestination(roomBHVMap[targetCoordinates].doorEast);
            roomBHVMap[targetCoordinates].doorEast.SetDestination(roomBHVMap[sourceCoordinates].doorWest);
        }
        else if(orientation == 2)
        {
            roomBHVMap[sourceCoordinates].doorNorth.SetDestination(roomBHVMap[targetCoordinates].doorSouth);
            roomBHVMap[targetCoordinates].doorSouth.SetDestination(roomBHVMap[sourceCoordinates].doorNorth);
        }
    }
}

