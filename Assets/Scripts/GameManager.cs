using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using TMPro;
using LevelGenerator;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    Program generator;
    public Dungeon createdDungeon;
    [SerializeField]
    Button startButton;
    [SerializeField]
    TextMeshProUGUI progressText;
    private IEnumerator coroutine;
    private bool isCompleted;
    private bool isInGame;

    public static GameManager instance = null;
    private List<TextAsset> maps = new List<TextAsset>();
    private List<TextAsset> rooms = new List<TextAsset>();
    private List<int> randomLevelList = new List<int>();
    private Map map = null;
    public bool createMaps = true; //If true, runs the AE to create maps. If false, loads the ones on the designated folders
    public AudioSource audioSource;
    public AudioClip bgMusic, fanfarreMusic;
    public TextMeshProUGUI keyText, roomText, levelText;
    public RoomBHV roomPrefab;
    public Transform roomsParent;  //Transform to hold rooms for leaner hierarchy view
    public RoomBHV[,] roomBHVMap; //2D array for easy room indexing
    public float roomSpacingX = 10.5f; //Spacing between rooms: X
    public float roomSpacingY = 6f; //Spacing between rooms: Y
    private string mapDirectory;
    //private static string[] maps = null;
    //private static string[] rooms = null;
    private int currentMapId = 0;
    private int currentTestBatchId = 0;
    //public string mapFilePath = "Assets/Data/map.txt"; //Path to load map data from
    //public string roomsFilePath = "Assets/Data/rooms.txt";
    public bool readRooms = true;
    public GameObject formMenu, endingScreen;

    public enum LevelPlayState { InProgress, Won, Lost, Skip, Quit }
    public static LevelPlayState state = LevelPlayState.InProgress;
    private static float secondsElapsed = 0;

    void Awake() {
        //Singleton
        if (instance == null) {
            instance = this;

            generator = new Program();

            audioSource = GetComponent<AudioSource>();

            readRooms = false;
            DontDestroyOnLoad(gameObject);
            AnalyticsEvent.GameStart();
            Debug.Log("Level Order");
            for (int i = 0; i < 6; ++i)
            {
                int aux;
                do
                {
                    aux = Random.Range(0, 6);
                }
                while (randomLevelList.Contains(aux));
                randomLevelList.Add(aux);
                Debug.Log(aux);
            }
            maps.Add(Resources.Load<TextAsset>("Batch0/Lizard"));
            maps.Add(Resources.Load<TextAsset>("Batch0/MyMoon"));
            maps.Add(Resources.Load<TextAsset>("Batch0/MyLizard"));
            maps.Add(Resources.Load<TextAsset>("Batch0/Dragon"));
            maps.Add(Resources.Load<TextAsset>("Batch0/MyDragon"));
            maps.Add(Resources.Load<TextAsset>("Batch0/Moon"));
        } else if (instance != this) {
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
    void Start() {
        //LoadNewLevel();
    }

    void InstantiateRooms() {
        for (int y = 0; y < Map.sizeY; y += 2) {
            for (int x = 0; x < Map.sizeX; x += 2) {
                InstantiateRoom(x, y);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        secondsElapsed += Time.deltaTime;
        if (!isInGame)
            if (generator.hasFinished)
            {
                instance.createdDungeon = generator.aux;
                if (startButton != null)
                    startButton.interactable = true;
            }
        if (isCompleted)
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

    void LoadMap(int mapId) {
        if (readRooms) { //deve ler também os tiles das salas?
            map = new Map(maps[mapId].text, rooms[mapId].text);
        } else { //apenas as salas, sem tiles
            map = new Map(maps[mapId].text);
        }
    }

    public Map GetMap() {
        return map;
    }

    void InstantiateRoom(int x, int y) {
        if (map.rooms[x, y] == null) {
            return;
        }
        Room room = map.rooms[x, y];
        RoomBHV newRoom = Instantiate(roomPrefab, roomsParent);
        roomBHVMap[x, y] = newRoom;
        if (x > 1) { // west
            if (map.rooms[x - 1, y] != null) {
                //Sets door
                newRoom.westDoor = map.rooms[x - 1, y].lockID;
                //Links room doors - assumes the rooms are given in a specific order from data: incr X, incr Y
                roomBHVMap[x, y].doorWest.SetDestination(roomBHVMap[x - 2, y].doorEast);
                roomBHVMap[x - 2, y].doorEast.SetDestination(roomBHVMap[x, y].doorWest);
            }
        }
        if (y > 1) { // north
            if (map.rooms[x, y - 1] != null) {
                //Sets door
                newRoom.northDoor = map.rooms[x, y - 1].lockID;
                //Links room doors - assumes the rooms are given in a specific order from data: incr X, incr Y
                roomBHVMap[x, y].doorNorth.SetDestination(roomBHVMap[x, y - 2].doorSouth);
                roomBHVMap[x, y - 2].doorSouth.SetDestination(roomBHVMap[x, y].doorNorth);
            }
        }
        if (x < Map.sizeX) { // east
            if (map.rooms[x + 1, y] != null) {
                //Sets door
                newRoom.eastDoor = map.rooms[x + 1, y].lockID;
            }
        }
        if (y < Map.sizeY) { // south
            if (map.rooms[x, y + 1] != null) {
                //Sets door
                newRoom.southDoor = map.rooms[x, y + 1].lockID;
            }
        }
        newRoom.x = x; //TODO: check use
        newRoom.y = y; //TODO: check use
        newRoom.availableKeyID = room.keyID; // Avaiable key to be collected in that room
        if (x == map.startX && y == map.startY) { // sala é a inicial
            newRoom.isStart = true;
        }
        if (x == map.endX && y == map.endY) { // sala é a final
            newRoom.isEnd = true;
        }
        //Sets room transform position
        newRoom.gameObject.transform.position = new Vector2(roomSpacingX * x, -roomSpacingY * y);
    }

    public void LoadNewLevel()
    {
        Time.timeScale = 1f;
        ChangeMusic(bgMusic);
        if (createMaps)
        {
            map = new Map(instance.createdDungeon);
        }
        else
        {
            if (maps != null)
            {
                /*Debug.Log("MapSize: " + maps.Count);
                foreach (TextAsset file in maps)
                {
                    Debug.Log("Map: " + file.text);
                }*/

                AnalyticsEvent.LevelStart(randomLevelList[currentMapId]);
                //Loads map from data
                LoadMap(randomLevelList[currentMapId]);
            }
            else
            {
                Debug.Log("Something is wrong with the map directory!");
            }
        }
        roomBHVMap = new RoomBHV[Map.sizeX, Map.sizeY];
        for (int x = 0; x < Map.sizeX; x++)
        {
            for (int y = 0; y < Map.sizeY; y++)
            {
                roomBHVMap[x, y] = null;
            }
        }
        InstantiateRooms();
        Player.instance.keys.Clear();
        Player.instance.usedKeys.Clear();
        Player.instance.AdjustCamera(map.startX, map.startY);
        Player.instance.SetRoom(map.startX, map.startY);
        UpdateLevelGUI();
        UpdateRoomGUI(map.startX, map.startY);
        OnStartMap(randomLevelList[currentMapId], currentTestBatchId, map);
    }

    private void OnStartMap (int id, int batch, Map map)
    {
        PlayerProfile.instance.OnMapStart(id, batch, map.rooms);
        PlayerProfile.instance.OnRoomEnter(map.startX, map.startY);
        Debug.Log("Started Profiling");
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
        //TODO save every gameplay data
        //TODO make it load a new level
        Debug.Log("MapID:" + randomLevelList[currentMapId]);
        Debug.Log("MapsLength:" + maps.Count);

        //Analytics for the level
        Dictionary<string, object> customParams = new Dictionary<string, object>();
        customParams.Add("seconds_played", secondsElapsed);
        customParams.Add("keys", Player.instance.keys.Count);
        customParams.Add("locks", Player.instance.usedKeys.Count);

        if (!createMaps)
            LoadForm();
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
        PlayerProfile.instance.OnMapComplete();
        if (!createMaps)
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
        {
            isCompleted = true;
        }
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);
        if(scene.name == "Level" || scene.name == "LevelGenerator")
        {
            coroutine = generator.CreateDungeonParallel(progressText);
            StartCoroutine(coroutine);
        }

        if(scene.name == "LevelGenerator")
        {
            isInGame = false;
            startButton = GameObject.Find("StartButton").GetComponent<Button>();
            //Level Generator Stuff
            startButton.interactable = false;
        }

        if (scene.name == "Level")
        {
            isInGame = true;
            startButton = null;
            isCompleted = false;

            Player pl = Player.instance;
            pl.cam = Camera.main;
            formMenu = GameObject.Find("Canvas").transform.Find("Form Questions").gameObject;
            keyText = GameObject.Find("KeyUIText").GetComponent<TextMeshProUGUI>();
            roomText = GameObject.Find("RoomUI").GetComponent<TextMeshProUGUI>();
            levelText = GameObject.Find("LevelUI").GetComponent<TextMeshProUGUI>();
            endingScreen = GameObject.Find("Canvas").transform.Find("FormPanel").gameObject;
            LoadNewLevel();
        }
    }

    void OnDestroy()
    {
        
    }
    public void LoadForm()
    {
        Time.timeScale = 0f;
        //Open a GUI here
        formMenu.SetActive(true);
        //TODO: Should check if there is a new batch, if not, set as inactive the continue button.
    }
    //Load a new batch of levels, if it exists
    public void LoadNewBatch()
    {
        formMenu.SetActive(false);
        currentTestBatchId++;
        currentMapId = 0;
        if (currentTestBatchId == 1)
        {
            readRooms = true;

            maps.Clear();
            maps = new List<TextAsset>();
            rooms.Clear();
            rooms = new List<TextAsset>();

            maps.Add(Resources.Load<TextAsset>("Batch1/Eagle"));
            maps.Add(Resources.Load<TextAsset>("Batch1/MyEagle"));
            maps.Add(Resources.Load<TextAsset>("Batch1/Lion"));
            maps.Add(Resources.Load<TextAsset>("Batch1/Snake"));
            maps.Add(Resources.Load<TextAsset>("Batch1/MyLion"));
            maps.Add(Resources.Load<TextAsset>("Batch1/MySnake"));

            rooms.Add(Resources.Load<TextAsset>("Batch1/EagleRoom"));
            rooms.Add(Resources.Load<TextAsset>("Batch1/MyEagleRoom"));
            rooms.Add(Resources.Load<TextAsset>("Batch1/LionRoom"));
            rooms.Add(Resources.Load<TextAsset>("Batch1/SnakeRoom"));
            rooms.Add(Resources.Load<TextAsset>("Batch1/MyLionRoom"));
            rooms.Add(Resources.Load<TextAsset>("Batch1/MySnakeRoom"));

            /*if (Directory.Exists(mapDirectory))
            {
                // This path is a directory
                ProcessDirectory(mapDirectory+currentTestBatchId, "map*", ref maps);
                ProcessDirectory(mapDirectory+currentTestBatchId, "room*", ref rooms);
            }*/
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            //LoadNewLevel();
        }
        else
        {
            //keyText.gameObject.SetActive(false);
            //roomText.gameObject.SetActive(false);
            endingScreen.SetActive(true);
        }
    }

    public void UpdateKeyGUI()
    {
        keyText.text = "x" + Player.instance.keys.Count;
    }

    public void UpdateRoomGUI(int x, int y)
    {
        roomText.text = "Sala: " + x/2 + "," + y/2;
    }

    public void UpdateLevelGUI()
    {
        int aux = currentMapId + 1 + (currentTestBatchId * maps.Count);
        levelText.text = "Nível: " + aux + "/12";
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
            audioSource.volume = 1.0f;
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();
    }
    
}
