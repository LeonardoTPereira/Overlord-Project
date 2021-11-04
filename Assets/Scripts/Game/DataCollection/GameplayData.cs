using Game.LevelManager;
using Game.EnemyGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game.GameManager;
using Game.NarrativeGenerator;
using UnityEngine;
using Util;

public struct CombatRoomInfo
{
    public int roomId;
    public bool hasEnemies;
    public int nEnemies;
    public List<int> enemiesIndex;
    public int playerInitHealth;
    public int playerFinalHealth;
    public int timeToExit;
}

public class GameplayData : MonoBehaviour
{
    private static string CSV = ".csv";
    private static string POST_DATA_URL = "http://damicore.icmc.usp.br/pag/data/upload.php?";
    private static int POST_QUESTIONS = 8;


    public static GameplayData instance = null;


    [SerializeField]
    private string profileString;
    [SerializeField]
    private string heatMapString;
    [SerializeField]
    private string levelProfileString;
    [SerializeField]
    private string detailedLevelProfileString;


    [SerializeField]
    public string sessionUID;


    [SerializeField]
    private List<int> preFormAnswers = new List<int>();


    [SerializeField]
    private string levelID = null;
    [SerializeField]
    private int chosenWeapon = -1;
    [SerializeField]
    private int elapsedTime = 0;
    // 0 if the player gave up or died, 1 if the player completed the level
    [SerializeField]
    private bool hasFinished = false;
    // 0 if the player gave up or completed the level, 1 if the player died
    [SerializeField]
    private bool hasDied = false;
    [SerializeField]
    private int totalKeys = 0;
    [SerializeField]
    private int collectedKeys = 0;
    [SerializeField]
    private int totalLocks = 0;
    [SerializeField]
    private int openedLocks = 0;
    [SerializeField]
    private int totalRooms = 0;
    [SerializeField]
    private int numberOfVisitedRooms = 0;
    [SerializeField]
    private int totalVisits = 0;
    [SerializeField]
    private int numberOfOverVisitedRooms = 0;
    [SerializeField]
    private int playerInitialHealth = 0;
    [SerializeField]
    private int playerFinalHealth = 0;
    [SerializeField]
    private int playerLostHealth = 0;
    [SerializeField]
    private int numberOfEnemies = 0;
    [SerializeField]
    private int numberOfKilledEnemies = 0;
    [SerializeField]
    private int numberOfNPCs = 0;
    [SerializeField]
    private int numberOfInteractedNPCs = 0;
    [SerializeField]
    public int totalTreasures = 0;
    [SerializeField]
    public int treasureCollected = 0;
    [SerializeField]
    public int maxCombo = 0;
    private List<int> postFormAnswers = new List<int>();


    [SerializeField]
    private int[,] heatMap;


    // Auxiliary variables
    private List<string> playedLevels = new List<string>();
    private Map map;
    private int attempts = 0;
    private bool pretestAnswered = false;
    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
    public List<Vector2Int> visitedRooms = new List<Vector2Int>();
    public int actualCombo = 0;
    private int curBatchId;


    // Player type classification
    private PlayerProfile playerProfile;
    private PlayerProfile givenPlayerProfile;


    // Enemy Generator Data
    protected List<CombatRoomInfo> combatInfoList;
    protected int difficultyLevel; // TODO SET IT WITH THE NARRATIVE JSON
    protected List<int> damageDoneByEnemy;
    protected int timesPlayerDied;
    public CombatRoomInfo actualRoomInfo;


    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
            combatInfoList = new List<CombatRoomInfo>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        // FIXME: utilizar uma ID única corretamente
        string dateTime = System.DateTime.Now.ToString();
        dateTime = dateTime.Replace("/", "-");
        dateTime = dateTime.Replace(" ", "-");
        dateTime = dateTime.Replace(":", "-");
        sessionUID = UnityEngine.Random.Range(0, 9999).ToString("00");
        sessionUID += "_";
        sessionUID += dateTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void OnEnable()
    {
        ProjectileController.enemyHitEventHandler += IncrementCombo;
        ProjectileController.playerHitEventHandler += ResetCombo;
        BombController.PlayerHitEventHandler += ResetCombo;
        EnemyController.playerHitEventHandler += ResetCombo;
        TreasureController.treasureCollectEvent += GetTreasure;
        GameManagerSingleton.NewLevelLoadedEventHandler += ResetMaxCombo;
        GameManagerSingleton.NewLevelLoadedEventHandler += ResetTreasure;
        GameManagerSingleton.GameStartEventHandler += OnGameStart;
        Player.EnterRoomEventHandler += OnRoomEnter;
        KeyBHV.KeyCollectEventHandler += OnGetKey;
        HealthController.PlayerIsDamagedEventHandler += OnEnemyDoesDamage;
        GameManagerSingleton.FinishMapEventHandler += OnMapComplete;
        PlayerController.PlayerDeathEventHandler += OnDeath;
        FormBHV.FormQuestionAnsweredEventHandler += OnFormAnswered;
        Player.ExitRoomEventHandler += OnRoomExit;
        DoorBHV.KeyUsedEventHandler += OnKeyUsed;
        GameManagerSingleton.StartMapEventHandler += OnMapStart;
        Manager.ProfileSelectedEventHandler += OnExperimentProfileSelected;
        ExperimentController.ProfileSelectedEventHandler += OnProfileSelected;
        EnemyController.KillEnemyEventHandler += OnKillEnemy;
        NPC.DialogueOpenEventHandler += OnInteractNPC;
    }

    protected void OnDisable()
    {
        ProjectileController.enemyHitEventHandler -= IncrementCombo;
        ProjectileController.playerHitEventHandler -= ResetCombo;
        BombController.PlayerHitEventHandler -= ResetCombo;
        EnemyController.playerHitEventHandler -= ResetCombo;
        TreasureController.treasureCollectEvent -= GetTreasure;
        GameManagerSingleton.NewLevelLoadedEventHandler -= ResetMaxCombo;
        GameManagerSingleton.NewLevelLoadedEventHandler -= ResetTreasure;
        GameManagerSingleton.GameStartEventHandler -= OnGameStart;
        Player.EnterRoomEventHandler -= OnRoomEnter;
        KeyBHV.KeyCollectEventHandler -= OnGetKey;
        HealthController.PlayerIsDamagedEventHandler -= OnEnemyDoesDamage;
        GameManagerSingleton.FinishMapEventHandler -= OnMapComplete;
        PlayerController.PlayerDeathEventHandler -= OnDeath;
        FormBHV.FormQuestionAnsweredEventHandler -= OnFormAnswered;
        Player.ExitRoomEventHandler -= OnRoomExit;
        DoorBHV.KeyUsedEventHandler -= OnKeyUsed;
        Manager.ProfileSelectedEventHandler -= OnProfileSelected;
        ExperimentController.ProfileSelectedEventHandler -= OnExperimentProfileSelected;
        EnemyController.KillEnemyEventHandler -= OnKillEnemy;
        NPC.DialogueOpenEventHandler -= OnInteractNPC;
    }

    //From FormBHV
    private void OnFormAnswered(object sender, FormAnsweredEventArgs eventArgs)
    {
        if (eventArgs.FormID == (int)Enums.FormEnum.PreTestForm)
            preFormAnswers.Add(eventArgs.AnswerValue);
        else if (eventArgs.FormID == (int)Enums.FormEnum.PostTestForm)
            postFormAnswers.Add(eventArgs.AnswerValue);
    }

    private void OnProfileSelected(object sender, ProfileSelectedEventArgs eventArgs)
    {
        playerProfile = eventArgs.PlayerProfile;
    }

    private void OnExperimentProfileSelected(object sender, ProfileSelectedEventArgs eventArgs)
    {
        givenPlayerProfile = eventArgs.PlayerProfile;
    }

    private void IncrementCombo(object sender, EventArgs eventArgs)
    {
        actualCombo++;
    }

    private void ResetCombo(object sender, EventArgs eventArgs)
    {
        if (actualCombo > maxCombo)
            maxCombo = actualCombo;
        actualCombo = 0;
    }

    private void ResetMaxCombo(object sender, EventArgs eventArgs)
    {
        actualCombo = 0;
        maxCombo = 0;
    }

    private void GetTreasure(object sender, TreasureCollectEventArgs eventArgs)
    {
        treasureCollected += eventArgs.Amount;
    }

    private void ResetTreasure(object sender, EventArgs eventArgs)
    {
        treasureCollected = 0;
    }

    //From KeyBHV
    private void OnGetKey(object sender, KeyCollectEventArgs eventArgs)
    {
        //Log
        collectedKeys++;
        //TODO also save key Index
    }

    //From DoorBHV
    private void OnKeyUsed(object sender, KeyUsedEventArgs eventArgs)
    {
        //Log
        openedLocks++;
        //Mais métricas - organiza em TAD
    }

    private void OnKillEnemy(object sender, EventArgs eventArgs)
    {
        numberOfKilledEnemies++;
    }

    private void OnInteractNPC(object sender, EventArgs eventArgs)
    {
        numberOfInteractedNPCs++;
    }

    private void OnEnemyDoesDamage(object sender, PlayerIsDamagedEventArgs eventArgs)
    {
        damageDoneByEnemy[eventArgs.EnemyIndex] += eventArgs.DamageDone;
    }

    //From DoorBHV
    private void OnRoomEnter(object sender, EnterRoomEventArgs eventArgs)
    {
        //Log
        //Mais métricas - organiza em TAD
        visitedRooms.Add(new Vector2Int(eventArgs.RoomCoordinates.X, eventArgs.RoomCoordinates.Y));

        // Collect the player health for level data
        if (visitedRooms.Count == 1)
        {
            playerInitialHealth = eventArgs.PlayerHealthWhenEntering;
        }
        playerFinalHealth = eventArgs.PlayerHealthWhenEntering;

        Debug.Log("VISITED: (x=" + eventArgs.RoomCoordinates.X + ",y=" + eventArgs.RoomCoordinates.Y);
        if (eventArgs.RoomHasEnemies)
        {
            actualRoomInfo.roomId = 10 * eventArgs.RoomCoordinates.X + eventArgs.RoomCoordinates.Y;
            actualRoomInfo.hasEnemies = eventArgs.RoomHasEnemies;
            actualRoomInfo.playerInitHealth = eventArgs.PlayerHealthWhenEntering;
            actualRoomInfo.nEnemies = eventArgs.RoomEnemiesIndex.Count;
            actualRoomInfo.enemiesIndex = eventArgs.RoomEnemiesIndex;
            actualRoomInfo.timeToExit = System.Convert.ToInt32(stopWatch.ElapsedMilliseconds);
        }
        else
            actualRoomInfo.roomId = -1;

        // Check the room coordinates to avoid division by zero
        if (eventArgs.RoomCoordinates.X != 0 && eventArgs.RoomCoordinates.Y != 0) {
            heatMap[eventArgs.RoomCoordinates.X / 2, eventArgs.RoomCoordinates.Y / 2]++;
        }
    }

    //From DoorBHV
    private void OnRoomExit(object sender, ExitRoomEventArgs eventArgs)
    {
        if (actualRoomInfo.roomId != -1)
        {
            actualRoomInfo.playerFinalHealth = eventArgs.PlayerHealthWhenExiting;
            actualRoomInfo.timeToExit = Convert.ToInt32(stopWatch.ElapsedMilliseconds) - actualRoomInfo.timeToExit;
            combatInfoList.Add(actualRoomInfo);
        }
    }

    //From inheritance
    private void OnApplicationQuit()
    {
        //Log
    }


    private void OnGameStart(object sender, EventArgs eventArgs)
    {
        profileString = "";
        heatMapString = "";
        levelProfileString = "";
        detailedLevelProfileString = "";
        // Enemy Generator Data
        combatInfoList = new List<CombatRoomInfo>();
        difficultyLevel = -1;
        timesPlayerDied = 0;
        hasFinished = false; //0 if player gave up, 1 if he completed the stage
        chosenWeapon = -1;
    }

    //From GameManager
    private void OnMapStart(object sender, StartMapEventArgs eventArgs)
    {
        if (playedLevels.Contains(eventArgs.MapName))
        {
            attempts++;
        }
        else
        {
            playedLevels.Add(eventArgs.MapName);
        }
        map = eventArgs.Map;
        curBatchId = eventArgs.MapBatch;

        // Initialize data for level data collection
        levelID = eventArgs.MapName;
        chosenWeapon = eventArgs.PlayerProjectileIndex;
        elapsedTime = 0;
        hasFinished = false;
        hasDied = false;
        totalKeys = map.NKeys;
        collectedKeys = 0;
        totalLocks = map.NLocks;
        openedLocks = 0;
        totalRooms = map.NRooms;
        numberOfVisitedRooms = 0;
        totalVisits = 0;
        numberOfOverVisitedRooms = 0;
        playerInitialHealth = 0;
        playerFinalHealth = 0;
        playerLostHealth = 0;
        numberOfEnemies = map.NEnemies;
        numberOfKilledEnemies = 0;
        numberOfNPCs = map.NNPCs;
        numberOfInteractedNPCs = 0;
        totalTreasures = GameManagerSingleton.instance.maxTreasure;
        treasureCollected = 0;
        maxCombo = 0;

        stopWatch.Reset();
        stopWatch.Start();

        heatMap = CreateHeatMap(map);

        combatInfoList = new List<CombatRoomInfo>();
        damageDoneByEnemy = new int[EnemyUtil.nBestEnemies].ToList();
    }

    private void EndTheLevel()
    {
        stopWatch.Stop();
        elapsedTime = stopWatch.Elapsed.Seconds;

        playerLostHealth = playerInitialHealth - playerFinalHealth;

        totalVisits = visitedRooms.Count;
        numberOfVisitedRooms = visitedRooms.Distinct().Count();
        numberOfOverVisitedRooms = visitedRooms
            .GroupBy(x => x.ToString())
            .Count(x => x.Count() > 1);

        ResetCombo(this, EventArgs.Empty);

        //Save to remote file
        SendProfileToServer(map);

        // Reset all values
        visitedRooms.Clear();
        postFormAnswers.Clear();
        damageDoneByEnemy.Clear();
    }

    //From TriforceBHV
    private void OnMapComplete(object sender, FinishMapEventArgs eventArgs)
    {
        hasFinished = true;
        EndTheLevel();
    }

    private void OnDeath(object sender, EventArgs eventArgs)
    {
        hasDied = true;
        playerFinalHealth = 0;
        timesPlayerDied++;

        if (actualRoomInfo.roomId != -1)
        {
            actualRoomInfo.playerFinalHealth = 0;
            actualRoomInfo.timeToExit = System.Convert.ToInt32(stopWatch.ElapsedMilliseconds) - actualRoomInfo.timeToExit;
            combatInfoList.Add(actualRoomInfo);
        }

        EndTheLevel();
    }


    private int[,] CreateHeatMap(Map currentMap)
    {
        int[,] heatMap = new int[(currentMap.Dimensions.Width + 1) / 2, (currentMap.Dimensions.Height + 1) / 2];
        for (int i = 0; i < currentMap.Dimensions.Width / 2; ++i)
        {
            //string aux = "";
            for (int j = 0; j < currentMap.Dimensions.Height / 2; ++j)
            {
                if (currentMap.DungeonPartByCoordinates.ContainsKey(new Coordinates(i * 2, j * 2)))
                {
                    heatMap[i, j] = 0;
                }
                else
                {
                    heatMap[i, j] = -1;
                }
            }
            //Debug.Log(aux);
        }
        //Debug.Log("Finished Creating HeatMap");
        return heatMap;
    }

    private void WrapProfileToString()
    {
        profileString = "";
        profileString += "Profile,";
        profileString += "ExperimentalProfile,";
        if (preFormAnswers.Count > 0)
        {
            int i = 0;
            foreach (int answer in preFormAnswers)
            {
                profileString += "PreQuestion " + i + ",";
                i++;
            }
            profileString += "\n";
            profileString += playerProfile.PlayerProfileEnum+",";
            profileString += givenPlayerProfile.PlayerProfileEnum+",";
            foreach (int answer in preFormAnswers)
            {
                profileString += answer + ",";
            }
        }
    }

    private void WrapLevelProfileToString()
    {
        levelProfileString = "";
        if (attempts == 0)
        {
            levelProfileString +=
                "player_id" + "," +
                "map_id" + "," +
                "chosen_weapon" + "," +
                "elapsed_time" + "," +
                "has_finished" + "," +
                "has_died" + "," +
                "total_keys" + "," +
                "collected_keys" + "," +
                "total_locks" + "," +
                "opened_locks" + "," +
                "total_rooms" + "," +
                "number_of_visited_rooms" + "," +
                "total_visits" + "," +
                "number_of_over_visited_rooms" + "," +
                "player_initial_health" + "," +
                "player_final_health" + "," +
                "player_lost_health" + "," +
                "number_of_enemies" + "," +
                "number_of_killed_enemies" + "," +
                "number_of_npcs" + "," +
                "number_of_interacted_npcs" + "," +
                "total_treasures" + "," +
                "collected_treasures" + "," +
                "max_combo" + ",";
            // for (int i = 0; i < EnemyUtil.nBestEnemies; ++i)
            // {
            //     levelProfileString += "Enemy" + i + "Damage,";
            // }
            if (postFormAnswers.Count > 0)
            {
                int i = 0;
                foreach (int answer in postFormAnswers)
                {
                    levelProfileString += "PostQuestion " + i + ",";
                    i++;
                }
            }
            else
            {
                for (int i = 0; i < POST_QUESTIONS; i++)
                {
                    levelProfileString += "PostQuestion " + i + ",";
                }
            }
            levelProfileString += "\n";
        }

        levelProfileString +=
            sessionUID + "," +
            levelID + "," +
            chosenWeapon + "," +
            elapsedTime + "," +
            hasFinished + "," +
            hasDied + "," +
            totalKeys + "," +
            collectedKeys + "," +
            totalLocks + "," +
            openedLocks + "," +
            totalRooms + "," +
            numberOfVisitedRooms + "," +
            totalVisits + "," +
            numberOfOverVisitedRooms + "," +
            playerInitialHealth + "," +
            playerFinalHealth + "," +
            playerLostHealth + "," +
            numberOfEnemies + "," +
            numberOfKilledEnemies + "," +
            numberOfNPCs + "," +
            numberOfInteractedNPCs + "," +
            totalTreasures + "," +
            treasureCollected + "," +
            maxCombo + ",";
        // for (int i = 0; i < EnemyUtil.nBestEnemies; ++i)
        // {
        //     levelProfileString += damageDoneByEnemy[i] + ",";
        // }
        if (postFormAnswers.Count > 0)
        {
            foreach (int answer in postFormAnswers)
            {
                levelProfileString += answer + ",";
            }
        }
        else
        {
            for (int i = 0; i < POST_QUESTIONS; i++)
            {
                levelProfileString += "-1,";
            }
        }
        levelProfileString += "\n";
    }

    private void WrapLevelDetailedCombatProfileToString()
    {
        detailedLevelProfileString += "map,RoomID,playerInitialHealth,PlayerFinalHealth,HealthLost,TimeToExit,hasEnemies,nEnemies,EnemiesIds,\n";
        foreach (CombatRoomInfo info in combatInfoList)
        {
            detailedLevelProfileString += levelID + ",";
            detailedLevelProfileString += info.roomId + ",";
            detailedLevelProfileString += info.playerInitHealth + ",";
            detailedLevelProfileString += info.playerFinalHealth + ",";
            detailedLevelProfileString += (info.playerFinalHealth - info.playerInitHealth) + ",";
            detailedLevelProfileString += info.timeToExit + ",";
            detailedLevelProfileString += info.hasEnemies + ",";
            detailedLevelProfileString += info.nEnemies + ",";
            foreach (int enemyId in info.enemiesIndex)
                detailedLevelProfileString += enemyId + ",";
            detailedLevelProfileString += "\n";
        }
    }

    private void WrapHeatMapToString(Map currentMap)
    {
        heatMapString = "";
        heatMapString += "map,\n";
        heatMapString += levelID + "\n";
        heatMapString += "Heatmap\n";
        for (int i = 0; i < currentMap.Dimensions.Width / 2; ++i)
        {
            for (int j = 0; j < currentMap.Dimensions.Height / 2; ++j)
            {
                heatMapString += heatMap[i, j].ToString() + ",";
            }
            heatMapString += "\n";
        }
        //Debug.Log(heatMapString);
    }


    //File name: BatchId, MapId, SessionUID
    //Player profile: N Visited Rooms, N Unique Visited Rooms, N Keys Taken, N Keys Used, Form Answer 1, Form Answer 2,Form Answer 3
    private void SendProfileToServer(Map currentMap)
    {
        WrapProfileToString();
        WrapHeatMapToString(currentMap);
        WrapLevelProfileToString();
        WrapLevelDetailedCombatProfileToString();
        // StartCoroutine(PostData("Map" + levelID, profileString, heatMapString, levelProfileString, detailedLevelProfileString)); //TODO: verificar corretamente como serão salvos os arquivos
        SaveToLocalFile("Map" + levelID, profileString, heatMapString, levelProfileString, detailedLevelProfileString);
        string UploadFilePath = GameplayData.instance.sessionUID;
    }

    private void SaveToLocalFile(string name, string stringData, string heatMapData, string levelData, string levelDetailedData)
    {
        string target = Application.streamingAssetsPath + "/PlayerData";
        if (!Directory.Exists(target))
        {
            Directory.CreateDirectory(target);
        }

        target += "/" + sessionUID + "-" + name;
        if (!pretestAnswered)
        {
            using (StreamWriter writer = new StreamWriter(target + "-Player" + CSV, true, Encoding.UTF8))
            {
                writer.Write(stringData);
                writer.Flush();
                writer.Close();
            }
            pretestAnswered = true;
        }

        using (StreamWriter writer = new StreamWriter(target + "-Heatmap" + CSV, true, Encoding.UTF8))
        {
            writer.Write(heatMapData);
            writer.Flush();
            writer.Close();
        }

        using (StreamWriter writer = new StreamWriter(target + "-Level" + CSV, true, Encoding.UTF8))
        {
            writer.Write(levelData);
            writer.Flush();
            writer.Close();
        }

        using (StreamWriter writer = new StreamWriter(target + "-Detailed" + CSV, true, Encoding.UTF8))
        {
            writer.Write(levelDetailedData);
            writer.Flush();
            writer.Close();
        }
    }

    private IEnumerator PostData(string name, string stringData, string heatMapData, string levelData, string levelDetailedData)
    {
        name = sessionUID + "-" + name;
        byte[] data = System.Text.Encoding.UTF8.GetBytes(stringData);
        byte[] heatMapBinary = System.Text.Encoding.UTF8.GetBytes(heatMapData);
        byte[] levelBinary = System.Text.Encoding.UTF8.GetBytes(levelData);
        byte[] levelDetailedBinary = System.Text.Encoding.UTF8.GetBytes(levelDetailedData);
        //This connects to a server side php script that will write the data
        //string post_url = postDataURL + "name=" + WWW.EscapeURL(name) + "&data=" + data ;
        string post_url = POST_DATA_URL;
        Debug.Log("LogName:" + name);
        WWWForm form = new WWWForm();
        form.AddField("name", sessionUID);
        form.AddBinaryData("data", data, name + "-Player" + CSV, "text/csv");
        form.AddBinaryData("heatmap", heatMapBinary, name + "-Heatmap" + CSV, "text/csv");
        form.AddBinaryData("level", levelBinary, name + "-Level" + CSV, "text/csv");
        form.AddBinaryData("detailed", levelDetailedBinary, name + "-Detailed" + CSV, "text/csv");

        // Post the URL to the site and create a download object to get the result.
        WWW data_post = new WWW(post_url, form);
        yield return data_post; // Wait until the download is done

        if (data_post.error != null)
        {
            print("There was an error saving data: " + data_post.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
    }
}
