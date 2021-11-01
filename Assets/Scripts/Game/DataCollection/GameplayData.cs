using Game.LevelManager;
using EnemyGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game.NarrativeGenerator;
using UnityEngine;
using Util;
using static Util.Enums;

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

    public static GameplayData instance = null;

    private int roomID = 0;

    private const string PostDataURL = "http://damicore.icmc.usp.br/pag/data/upload.php?";
    private int attemptOnLevelNumber, cumulativeAttempts; //TODO: entender o por quê desse int

    [SerializeField]
    public string sessionUID;
    [SerializeField]
    private string profileString, heatMapString, levelProfileString, detailedLevelProfileString;

    [SerializeField]
    private int mapCount = 0;
    [SerializeField]
    private int curBatchId;
    [SerializeField]
    private string curMapName = null;

    [SerializeField]
    public List<Vector2Int> visitedRooms = new List<Vector2Int>();
    [SerializeField]
    private int mapVisitedCount = 0;
    [SerializeField]
    public int mapVisitedCountUnique = 0;
    [SerializeField]
    private int keysTaken = 0;
    [SerializeField]
    private int keysUsed = 0;
    [SerializeField]
    private List<int> preFormAnswers = new List<int>();
    [SerializeField]
    private List<int> postFormAnswers = new List<int>();
    [SerializeField]
    private int secondsToFinish = 0;
    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
    [SerializeField]
    private int[,] heatMap;

    public int actualCombo, maxCombo;
    public int treasureCollected;
    protected int weaponUsed;

    //Enemy Generator Data
    protected List<CombatRoomInfo> combatInfoList;
    protected int difficultyLevel; //TODO SET IT WITH THE NARRATIVE JSON
    protected List<int> damageDoneByEnemy;
    protected int timesPlayerDied;
    public bool HasFinished { get; set; } //0 if player gave up, 1 if he completed the stage 
    public CombatRoomInfo actualRoomInfo;

    private string result;

    private PlayerProfile playerProfile;
    private PlayerProfile givenPlayerProfile;

    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
            combatInfoList = new List<CombatRoomInfo>();
            actualCombo = 0;
            maxCombo = 0;
            treasureCollected = 0;
            weaponUsed = -1;
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

        attemptOnLevelNumber = 0;
        cumulativeAttempts = 0;
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
        GameManager.NewLevelLoadedEventHandler += ResetMaxCombo;
        GameManager.NewLevelLoadedEventHandler += ResetTreasure;
        GameManager.GameStartEventHandler += OnGameStart;
        Player.EnterRoomEventHandler += OnRoomEnter;
        KeyBHV.KeyCollectEventHandler += OnGetKey;
        HealthController.PlayerIsDamagedEventHandler += OnEnemyDoesDamage;
        GameManager.FinishMapEventHandler += OnMapComplete;
        PlayerController.PlayerDeathEventHandler += OnDeath;
        FormBHV.FormQuestionAnsweredEventHandler += OnFormAnswered;
        Player.ExitRoomEventHandler += OnRoomExit;
        DoorBHV.KeyUsedEventHandler += OnKeyUsed;
        GameManager.StartMapEventHandler += OnMapStart;
        Manager.ProfileSelectedEventHandler += OnExperimentProfileSelected;
        ExperimentController.ProfileSelectedEventHandler += OnProfileSelected;
    }

    protected void OnDisable()
    {
        ProjectileController.enemyHitEventHandler -= IncrementCombo;
        ProjectileController.playerHitEventHandler -= ResetCombo;
        BombController.PlayerHitEventHandler -= ResetCombo;
        EnemyController.playerHitEventHandler -= ResetCombo;
        TreasureController.treasureCollectEvent -= GetTreasure;
        GameManager.NewLevelLoadedEventHandler -= ResetMaxCombo;
        GameManager.NewLevelLoadedEventHandler -= ResetTreasure;
        GameManager.GameStartEventHandler -= OnGameStart;
        Player.EnterRoomEventHandler -= OnRoomEnter;
        KeyBHV.KeyCollectEventHandler -= OnGetKey;
        HealthController.PlayerIsDamagedEventHandler -= OnEnemyDoesDamage;
        GameManager.FinishMapEventHandler -= OnMapComplete;
        PlayerController.PlayerDeathEventHandler -= OnDeath;
        FormBHV.FormQuestionAnsweredEventHandler -= OnFormAnswered;
        Player.ExitRoomEventHandler -= OnRoomExit;
        DoorBHV.KeyUsedEventHandler -= OnKeyUsed;
        Manager.ProfileSelectedEventHandler -= OnProfileSelected;
        ExperimentController.ProfileSelectedEventHandler -= OnExperimentProfileSelected;
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

    private void OnGameStart(object sender, EventArgs eventArgs)
    {
        profileString = "";
        heatMapString = "";
        levelProfileString = "";
        detailedLevelProfileString = "";
        mapCount = 0;
        visitedRooms = new List<Vector2Int>();
        mapVisitedCount = 0;
        mapVisitedCountUnique = 0;
        keysTaken = 0;
        keysUsed = 0;
        postFormAnswers = new List<int>();
        secondsToFinish = 0;
        stopWatch = new System.Diagnostics.Stopwatch();
        //Enemy Generator Data
        combatInfoList = new List<CombatRoomInfo>();
        difficultyLevel = -1;
        timesPlayerDied = 0;
        HasFinished = false; //0 if player gave up, 1 if he completed the stage 
        weaponUsed = -1;
    }

    //From DoorBHV
    private void OnRoomEnter(object sender, EnterRoomEventArgs eventArgs)
    {
        //Log
        //Mais métricas - organiza em TAD
        visitedRooms.Add(new Vector2Int(eventArgs.RoomCoordinates.X, eventArgs.RoomCoordinates.Y));
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

    //From DoorBHV
    private void OnKeyUsed(object sender, KeyUsedEventArgs eventArgs)
    {
        //Log
        keysUsed++;
        //Mais métricas - organiza em TAD
    }

    //From GameManager
    private void OnMapStart(object sender, StartMapEventArgs eventArgs)
    {
        HasFinished = false;
        mapCount++;
        if (curMapName != null)
            if (curMapName != eventArgs.MapName)
            {
                attemptOnLevelNumber = 1;
                timesPlayerDied = 0;
            }
            else
                attemptOnLevelNumber++;
        cumulativeAttempts++;
        curMapName = eventArgs.MapName;
        curBatchId = eventArgs.MapBatch;
        stopWatch.Start();
        heatMap = CreateHeatMap(eventArgs.Map);
        combatInfoList = new List<CombatRoomInfo>();
        damageDoneByEnemy = new int[EnemyUtil.nBestEnemies].ToList();
        //Debug.Log("On Map Start Profilling Called");
        weaponUsed = eventArgs.PlayerProjectileIndex;
        //Log
        //Mais métricas - organiza em TAD

    }

    //From inheritance
    private void OnApplicationQuit()
    {
        //Log
    }

    //From TriforceBHV
    private void OnMapComplete(object sender, FinishMapEventArgs eventArgs)
    {
        HasFinished = true;
        stopWatch.Stop();
        secondsToFinish = stopWatch.Elapsed.Seconds;
        stopWatch.Reset();
        //Log
        //Mais métricas - organiza em TAD, agrega dados do nível
        //visitedRooms = visitedRooms.Distinct();
        mapVisitedCount = visitedRooms.Count;
        mapVisitedCountUnique = visitedRooms.Distinct().Count();
        ResetCombo(this, EventArgs.Empty);

        //HasFinished = victory;
        //Save to remote file
        SendProfileToServer(eventArgs.Map);
        //Reset all values
        visitedRooms.Clear();
        postFormAnswers.Clear();
        keysTaken = 0;
        keysUsed = 0;
        profileString = "";
        damageDoneByEnemy.Clear();
    }

    //From KeyBHV
    private void OnGetKey(object sender, KeyCollectEventArgs eventArgs)
    {
        //Log
        keysTaken++;
        //TODO also save key Index
    }

    //From FormBHV
    private void OnFormAnswered(object sender, FormAnsweredEventArgs eventArgs)
    {
        if (eventArgs.FormID == (int)FormEnum.PreTestForm)
            preFormAnswers.Add(eventArgs.AnswerValue);
        else if (eventArgs.FormID == (int)FormEnum.PostTestForm)
            postFormAnswers.Add(eventArgs.AnswerValue);
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
        levelProfileString += "map,attempt,cumulativeAttempt,mapCount,uniquemap,keys,locks,time,maxCombo,treasure,weapon,difficulty,deaths,Victory?,";
        for (int i = 0; i < EnemyUtil.nBestEnemies; ++i)
            levelProfileString += "Enemy" + i + "Damage,";
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
            levelProfileString += "NoPostQuestions,";
        levelProfileString += "\n";

        levelProfileString += curMapName + "," + attemptOnLevelNumber + "," + cumulativeAttempts + "," + mapVisitedCount + "," + mapVisitedCountUnique + "," + keysTaken +
            "," + keysUsed + "," + secondsToFinish + "," + maxCombo + "," +
            treasureCollected + "," + weaponUsed + "," + difficultyLevel + "," + timesPlayerDied + "," + HasFinished + ",";
        for (int i = 0; i < EnemyUtil.nBestEnemies; ++i)
            levelProfileString += damageDoneByEnemy[i] + ",";
        if (postFormAnswers.Count > 0)
        {
            foreach (int answer in postFormAnswers)
            {
                levelProfileString += answer + ",";
            }
        }
        else
            levelProfileString += "-1,";
        levelProfileString += "\n";
    }

    private void WrapLevelDetailedCombatProfileToString()
    {
        detailedLevelProfileString += "map,attemptOnLevel,cumulativeAttempt,RoomID:,playerInitialHealth,PlayerFinalHealth,HealthLost,TimeToExit,hasEnemies,nEnemies,EnemiesIds,\n";
        foreach (CombatRoomInfo info in combatInfoList)
        {
            detailedLevelProfileString += curMapName + "," + attemptOnLevelNumber + "," + cumulativeAttempts + ",";
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
        heatMapString += "map,attempt,cumulativeAttempt\n";
        heatMapString += curMapName + "," + attemptOnLevelNumber + "," + cumulativeAttempts + "\n";
        heatMapString += "Heatmap:\n";
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
        StartCoroutine(PostData("Map" + curMapName, profileString, heatMapString, levelProfileString, detailedLevelProfileString)); //TODO: verificar corretamente como serão salvos os arquivos
        //saveToLocalFile("Map" + curMapName, profileString, heatMapString, levelProfileString, detailedLevelProfileString);
        string UploadFilePath = GameplayData.instance.sessionUID;

    }

    private void saveToLocalFile(string name, string stringData, string heatMapData, string levelData, string levelDetailedData)
    {
        if (!Directory.Exists(Application.streamingAssetsPath + "/PlayerData"))
            Directory.CreateDirectory(Application.streamingAssetsPath + "/PlayerData");
        if (cumulativeAttempts == 1)
        {
            using (StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/PlayerData/" + sessionUID + "Player" + name + ".csv", true, Encoding.UTF8))
            {
                writer.Write(stringData);
                writer.Flush();
                writer.Close();
            }
        }

        using (StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/PlayerData/" + sessionUID + "HM" + name + ".csv", true, Encoding.UTF8))
        {
            writer.Write(heatMapData);
            writer.Flush();
            writer.Close();
        }

        using (StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/PlayerData/" + sessionUID + "Level" + name + ".csv", true, Encoding.UTF8))
        {
            writer.Write(levelData);
            writer.Flush();
            writer.Close();
        }

        using (StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/PlayerData/" + sessionUID + "Detailed" + name + ".csv", true, Encoding.UTF8))
        {
            writer.Write(levelDetailedData);
            writer.Flush();
            writer.Close();
        }

    }

    private IEnumerator PostData(string name, string stringData, string heatMapData, string levelData, string levelDetailedData)
    {
        name = sessionUID + name;
        byte[] data = System.Text.Encoding.UTF8.GetBytes(stringData);
        byte[] heatMapBinary = System.Text.Encoding.UTF8.GetBytes(heatMapData);
        byte[] levelBinary = System.Text.Encoding.UTF8.GetBytes(levelData);
        byte[] levelDetailedBinary = System.Text.Encoding.UTF8.GetBytes(levelDetailedData);
        //This connects to a server side php script that will write the data
        //string post_url = postDataURL + "name=" + WWW.EscapeURL(name) + "&data=" + data ;
        string post_url = PostDataURL;
        Debug.Log("LogName:" + name);
        WWWForm form = new WWWForm();
        form.AddField("name", sessionUID);
        form.AddBinaryData("data", data, name + ".csv", "text/csv");
        form.AddBinaryData("heatmap", heatMapBinary, "HM" + name + ".csv", "text/csv");
        form.AddBinaryData("level", levelBinary, "Level" + name + ".csv", "text/csv");
        form.AddBinaryData("detailed", levelDetailedBinary, "Detailed" + name + ".csv", "text/csv");

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

    private void OnEnemyDoesDamage(object sender, PlayerIsDamagedEventArgs eventArgs)
    {
        damageDoneByEnemy[eventArgs.EnemyIndex] += eventArgs.DamageDone;
    }

    private void OnDeath(object sender, EventArgs eventArgs)
    {
        if (actualRoomInfo.roomId != -1)
        {
            actualRoomInfo.playerFinalHealth = 0;
            actualRoomInfo.timeToExit = System.Convert.ToInt32(stopWatch.ElapsedMilliseconds) - actualRoomInfo.timeToExit;
            combatInfoList.Add(actualRoomInfo);
        }
        timesPlayerDied++;
    }
}
