using EnemyGenerator;
using LevelGenerator;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Foldout("Scriptable Objects"), Header("Set With All Possible Treasures")]
    public TreasureRuntimeSetSO treasureSet;
    [Foldout("Scriptable Objects"), Header("Set With All Possible Projectiles")]
    public ProjectileTypeRuntimeSetSO projectileSet;
    [Foldout("Scriptable Objects"), ReadOnly, Header("Current Projectile")]
    public ProjectileTypeSO projectileType;
    protected Program generator;
    public Dungeon createdDungeon;
    [Separator("Other Stuff")]
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
    private int currentMapId = 0;
    private int currentTestBatchId = 0;
    public bool readRooms = true;
    public GameObject gameOverScreen, victoryScreen, introScreen, gameUI;

    private static float secondsElapsed = 0;

    public bool createEnemy, survivalMode, enemyMode;
    public EnemyLoader enemyLoader;
    public string chosenDifficultyFileName;
    public HealthUI healthUI;
    public KeyUI keyUI;

    public int nExecutions, nBatches;

    public static event EventHandler newLevelLoadedEventHandler;

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
            AnalyticsEvent.GameStart();
            for (int i = 0; i < 6; ++i)
            {
                int aux;
                do
                {
                    aux = UnityEngine.Random.Range(0, 6);
                }
                while (randomLevelList.Contains(aux));
                randomLevelList.Add(aux);
            }

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
        PlayerProfile.instance.OnGameStart();
        //LoadNewLevel();
        //panelIntro.SetActive(true);   foi comentado
    }

    void InstantiateRooms()
    {
        foreach (DungeonPart currentPart in map.dungeonPartByCoordinates.Values)
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

    void LoadMap(string mapFile)
    {
        map = new Map(mapFile, null, mapFileMode);
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
        if (IsTopEdge(dungeonRoom.coordinates))
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

    public void LoadNewLevel(string mapFile, string difficultyFileName)
    {
        //TODO fazer alguma lógica no arquivo de dificuldade para definir uma dificuldade geral do dungeon?
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

        roomBHVMap = new Dictionary<Coordinates, RoomBHV>();

        InstantiateRooms();
        ConnectRoooms();
        Player.instance.keys.Clear();
        Player.instance.usedKeys.Clear();
        Player.instance.AdjustCamera(map.startRoomCoordinates, (map.dungeonPartByCoordinates[map.startRoomCoordinates] as DungeonRoom).Dimensions.Width);


        UpdateLevelGUI();
        OnStartMap(mapFile, currentTestBatchId, map);
    }

    private void OnStartMap(string mapName, int batch, Map map)
    {
        PlayerProfile.instance.OnMapStart(mapName, batch, map, projectileSet.Items.IndexOf(projectileType));
        PlayerProfile.instance.OnRoomEnter(map.startRoomCoordinates, roomBHVMap[map.startRoomCoordinates].hasEnemies, roomBHVMap[map.startRoomCoordinates].enemiesIndex, Player.instance.GetComponent<PlayerController>().GetHealth());
    }

    void OnApplicationQuit()
    {
        AnalyticsEvent.GameOver();
    }

    public void LevelComplete()
    {
        ChangeMusic(fanfarreMusic);
        Time.timeScale = 0f;
        gameUI.SetActive(false);
        //TODO save every gameplay data
        //TODO make it load a new level

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

    }

    public void CheckEndOfBatch()
    {
        PlayerProfile.instance.OnMapComplete(map);
        isCompleted = true;
    }

    public void EndGame()
    {
        PlayerProfile.instance.OnMapComplete(map);
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        LevelLoaderBHV.loadLevelButtonEventHandler += PlayGameOnDifficulty;
        WeaponLoaderBHV.loadWeaponButtonEventHandler += SetProjectileSO;
        PostFormMenuBHV.postFormButtonEventHandler += PlayGameOnDifficulty;

        DungeonEntrance.loadLevelEventHandler += PlayGameOnDifficulty;
    }
    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        LevelLoaderBHV.loadLevelButtonEventHandler -= PlayGameOnDifficulty;
        WeaponLoaderBHV.loadWeaponButtonEventHandler -= SetProjectileSO;
        PostFormMenuBHV.postFormButtonEventHandler -= PlayGameOnDifficulty;

        DungeonEntrance.loadLevelEventHandler -= PlayGameOnDifficulty;
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

            if (enemyMode)
                enemyLoader.LoadEnemies(chosenDifficultyFileName);

            Player pl = Player.instance;
            pl.cam = Camera.main;
            //Recover health
            pl.gameObject.GetComponent<PlayerController>().ResetHealth();
            gameUI.SetActive(true);
            healthUI = gameUI.GetComponentInChildren<HealthUI>();
            keyUI = gameUI.GetComponentInChildren<KeyUI>();
            OnLevelLoadedEvents();
            LoadNewLevel(currentLevel, chosenDifficultyFileName);
        }
        if (scene.name == "Main")
        {
            introScreen.SetActive(true);
        }
    }

    public void UpdateLevelGUI()
    {
        healthUI.CreateHeartImage();
        keyUI.CreateKeyImage();
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
    public void GameOver()
    {
        //Time.timeScale = 1f; comentado por Luana e Paolo
        //gameUI.SetActive(false);
        //gameOverScreen.SetActive(true);

        SceneManager.LoadScene("LuanaPaolo");
    }

    public void MainMenu()
    {
        PlayerProfile.instance.OnGameStart();
        SceneManager.LoadScene("Main");
    }

    public void SetLevelMode(string fileName, string difficultyFileName)
    {
        chosenDifficultyFileName = difficultyFileName;
        currentLevel = fileName;
        levelSetNames.Remove(fileName);
    }

    public void PlayGameOnDifficulty(object sender, LevelLoadEventArgs args)
    {
        instance.SetLevelMode(args.LevelFile, args.EnemyFile);
    }

    public bool HasMoreLevels()
    {
        if (levelSetNames.Count > 0)
            return true;
        return false;
    }

    //TODO checar se experimento vai ser continuo ou não
    //Se for, precisa mudar essa lógica pra carregar o arquivo de inimigos
    /*public LevelConfigSO PickNextLevel()
    {
        LevelConfigSO curLevel = null;
        string nextLevelCandidate = levelSetNames[0];
        bool hasFound = false;
        if (PlayerProfile.instance.HasFinished)
        {
            foreach (string levelName in levelSetNames)
            {
                curLevel = levelSet.Items.Find(x => (x.fileName.CompareTo(levelName) == 0));
                if ((int)curLevel.enemyDifficultyInDungeon == chosenDifficulty)
                {
                    nextLevelCandidate = curLevel.fileName;
                    hasFound = true;
                    break;
                }
            }
            if (!hasFound)
            {
                foreach (string levelName in levelSetNames)
                {
                    curLevel = levelSet.Items.Find(x => (x.fileName.CompareTo(levelName) == 0));
                    if (Math.Abs((int)curLevel.enemyDifficultyInDungeon - chosenDifficulty) == 1)
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
        else
        {
            foreach (string levelName in levelSetNames)
            {
                curLevel = levelSet.Items.Find(x => (x.fileName.CompareTo(levelName) == 0));
                if ((chosenDifficulty - 1) == (int)curLevel.enemyDifficultyInDungeon)
                {
                    nextLevelCandidate = curLevel.fileName;
                    hasFound = true;
                    break;
                }
            }
            if (!hasFound)
            {
                foreach (string levelName in levelSetNames)
                {
                    curLevel = levelSet.Items.Find(x => (x.fileName.CompareTo(levelName) == 0));
                    if ((chosenDifficulty - 2) == (int)curLevel.enemyDifficultyInDungeon)
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
    }*/

    public void OnLevelLoadedEvents()
    {
        newLevelLoadedEventHandler(this, EventArgs.Empty);
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

