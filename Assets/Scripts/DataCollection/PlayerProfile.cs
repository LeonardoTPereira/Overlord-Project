using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EnemyGenerator;
using System.IO;
using System.Text;

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

public class PlayerProfile : MonoBehaviour {

    public static PlayerProfile instance = null;

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
    protected int difficultyLevel;
    protected List<int> damageDoneByEnemy;
    protected int timesPlayerDied;
    public bool HasFinished { get; set; } //0 if player gave up, 1 if he completed the stage 
    public CombatRoomInfo actualRoomInfo;

    private string result;

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
    void Start () {
        // FIXME: utilizar uma ID única corretamente
        string dateTime = System.DateTime.Now.ToString();
        dateTime = dateTime.Replace("/", "-");
        dateTime = dateTime.Replace(" ", "-");
        dateTime = dateTime.Replace(":", "-");
        sessionUID = Random.Range(0, 9999).ToString("00");
        sessionUID += "_";
        sessionUID += dateTime;

        attemptOnLevelNumber = 0;
        cumulativeAttempts = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void OnEnable()
    {
        ProjectileController.hitEnemyEvent += IncrementCombo;
        ProjectileController.hitPlayerEvent += ResetCombo;
        BombController.hitPlayerEvent += ResetCombo;
        EnemyController.hitPlayerEvent += ResetCombo;
        TreasureController.collectTreasureEvent += GetTreasure;
        GameManager.newLevelLoadedEvent += ResetMaxCombo;
        GameManager.newLevelLoadedEvent += ResetTreasure;
    }

    protected void OnDisable()
    {
        ProjectileController.hitEnemyEvent -= IncrementCombo;
        ProjectileController.hitPlayerEvent -= ResetCombo;
        BombController.hitPlayerEvent -= ResetCombo;
        EnemyController.hitPlayerEvent -= ResetCombo;
        TreasureController.collectTreasureEvent -= GetTreasure;
        GameManager.newLevelLoadedEvent -= ResetMaxCombo;
        GameManager.newLevelLoadedEvent -= ResetTreasure;
    }
    public void IncrementCombo()
    {
        actualCombo++;
    }

    public void ResetCombo()
    {
        if (actualCombo > maxCombo)
            maxCombo = actualCombo;
        actualCombo = 0;
    }

    public void ResetMaxCombo()
    {
        actualCombo = 0;
        maxCombo = 0;
    }

    public void GetTreasure(int value)
    {
        treasureCollected += value;
    }

    public void ResetTreasure()
    {
        treasureCollected = 0;
    }
    public void OnGameStart()
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

    //Events
    //From DoorBHV
    public void OnRoomFailEnter(Vector2Int offset)
    {
        //Log
        //Mais métricas - organiza em TAD
    }

    //From DoorBHV
    public void OnRoomEnter (int x, int y, bool hasEnemies, List<int> enemyList, int playerHealth)
    {
        //Log
        //Mais métricas - organiza em TAD
        heatMap[x / 2, y / 2]++;
        visitedRooms.Add(new Vector2Int(x, y));
        if (hasEnemies)
        {
            actualRoomInfo.roomId = 10 * x + y;
            actualRoomInfo.hasEnemies = hasEnemies;
            actualRoomInfo.playerInitHealth = playerHealth;
            actualRoomInfo.nEnemies = enemyList.Count;
            actualRoomInfo.enemiesIndex = enemyList;
            actualRoomInfo.timeToExit = System.Convert.ToInt32(stopWatch.ElapsedMilliseconds);
        }
        else
            actualRoomInfo.roomId = -1;
    }

    //From DoorBHV
    public void OnRoomFailExit(Vector2Int offset)
    {
        //Log
        //Mais métricas - organiza em TAD
    }

    //From DoorBHV
    public void OnRoomExit(Vector2Int offset, int playerHealth)
    {
        if(actualRoomInfo.roomId != -1)
        {
            actualRoomInfo.playerFinalHealth = playerHealth;
            actualRoomInfo.timeToExit = System.Convert.ToInt32(stopWatch.ElapsedMilliseconds) - actualRoomInfo.timeToExit;
            combatInfoList.Add(actualRoomInfo);
        }
        //Log
        //Mais métricas - organiza em TAD
    }

    //From DoorBHV
    public void OnKeyUsed(int id)
    {
        //Log
        keysUsed++;
        //Mais métricas - organiza em TAD
    }

    //From GameManager
    public void OnMapStart (string name, int batch, Room[,] rooms, int difficulty, int weapon)
    {
        HasFinished = false;
        mapCount++;
        if (curMapName != null)
            if (curMapName != name)
            {
                attemptOnLevelNumber = 1;
                timesPlayerDied = 0;
            }
            else
                attemptOnLevelNumber++;
        cumulativeAttempts++;
        curMapName = name;
        curBatchId = batch;
        stopWatch.Start();
        heatMap = CreateHeatMap(rooms);
        combatInfoList = new List<CombatRoomInfo>();
        damageDoneByEnemy = new int[EnemyUtil.nBestEnemies].ToList();
        difficultyLevel = difficulty;
        //Debug.Log("On Map Start Profilling Called");
        weaponUsed = weapon;
        //Log
        //Mais métricas - organiza em TAD
        
    }

    //From inheritance
    private void OnApplicationQuit()
    {
        //Log
    }

    //From TriforceBHV
    public void OnMapComplete ()
    {
        stopWatch.Stop();
        secondsToFinish = stopWatch.Elapsed.Seconds;
        stopWatch.Reset();
        //Log
        //Mais métricas - organiza em TAD, agrega dados do nível
        //visitedRooms = visitedRooms.Distinct();
        mapVisitedCount = visitedRooms.Count;
        mapVisitedCountUnique = visitedRooms.Distinct().Count();
        ResetCombo();

        //HasFinished = victory;
        //Save to remote file
        SendProfileToServer();
        //Reset all values
        visitedRooms.Clear();
        postFormAnswers.Clear();
        keysTaken = 0;
        keysUsed = 0;
        profileString = "";
        damageDoneByEnemy.Clear();
    }

    //From KeyBHV
    public void OnGetKey (int id)
    {
        //Log
        keysTaken++;
        //Mais métricas - organiza em TAD
    }

    //From FormBHV
    public void OnFormAnswered(int answer, int formID)
    {
        //Log
        //TODO FIX THIS WITH AN ENUM OR SOMETHING
        //0 PreTest 1 PostTest
        if(formID == 0)
            preFormAnswers.Add(answer);
        else if(formID == 1)
            postFormAnswers.Add(answer);
    }

    private void WrapProfileToString ()
    {
        profileString = "";
        if (preFormAnswers.Count > 0)
        {
            int i = 0;
            foreach (int answer in preFormAnswers)
            {
                profileString += "PreQuestion "+i+",";
                i++;
            }
            profileString += "\n";
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
            levelProfileString += "Enemy"+i+"Damage,";
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

        levelProfileString += curMapName+","+ attemptOnLevelNumber + ","+ cumulativeAttempts + "," + mapVisitedCount + ","+ mapVisitedCountUnique + "," + keysTaken +
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


    private void WrapHeatMapToString()
    {
        heatMapString = "";
        heatMapString += "map,attempt,cumulativeAttempt\n";
        heatMapString += curMapName + "," + attemptOnLevelNumber + "," + cumulativeAttempts + "\n";
        heatMapString += "Heatmap:\n";
        for (int i = 0; i < Map.sizeX / 2; ++i)
        {
            for (int j = 0; j < Map.sizeY / 2; ++j)
            {
                heatMapString += heatMap[i, j].ToString()+",";
            }
            heatMapString += "\n";
        }
        //Debug.Log(heatMapString);
    }
    //File name: BatchId, MapId, SessionUID
    //Player profile: N Visited Rooms, N Unique Visited Rooms, N Keys Taken, N Keys Used, Form Answer 1, Form Answer 2,Form Answer 3
    private void SendProfileToServer ()
    {
        WrapProfileToString();
        WrapHeatMapToString();
        WrapLevelProfileToString();
        WrapLevelDetailedCombatProfileToString();
        StartCoroutine(PostData("Map" + curMapName, profileString, heatMapString, levelProfileString, detailedLevelProfileString)); //TODO: verificar corretamente como serão salvos os arquivos
        //saveToLocalFile("Map" + curMapName, profileString, heatMapString, levelProfileString, detailedLevelProfileString);
        string UploadFilePath = PlayerProfile.instance.sessionUID;


    }

    void saveToLocalFile(string name, string stringData, string heatMapData, string levelData, string levelDetailedData)
    {
        if (!Directory.Exists(Application.streamingAssetsPath+"/PlayerData"))
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

    IEnumerator PostData(string name, string stringData, string heatMapData, string levelData, string levelDetailedData)
    {
        name = sessionUID + name;
        byte[] data = System.Text.Encoding.UTF8.GetBytes(stringData);
        byte[] heatMapBinary = System.Text.Encoding.UTF8.GetBytes(heatMapData);
        byte[] levelBinary = System.Text.Encoding.UTF8.GetBytes(levelData);
        byte[] levelDetailedBinary = System.Text.Encoding.UTF8.GetBytes(levelDetailedData);
        //This connects to a server side php script that will write the data
        //string post_url = postDataURL + "name=" + WWW.EscapeURL(name) + "&data=" + data ;
        string post_url = PostDataURL;
        Debug.Log("LogName:"+name);
        WWWForm form = new WWWForm();
        form.AddField("name", sessionUID);
        form.AddBinaryData("data", data, name + ".csv", "text/csv");
        form.AddBinaryData("heatmap", heatMapBinary, "HM"+name + ".csv", "text/csv");
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

    public int[,] CreateHeatMap(Room[,] rooms)
    {
        int[,] heatMap = new int[Map.sizeX / 2, Map.sizeY / 2];
        for (int i = 0; i < Map.sizeX / 2; ++i)
        {
            //string aux = "";
            for (int j = 0; j < Map.sizeY / 2; ++j)
            {
                if (rooms[i * 2, j * 2] == null)
                {
                    heatMap[i, j] = -1;
                    //aux += "-1";
                }
                else
                {
                    heatMap[i, j] = 0;
                    //aux += "0";
                }
            }
            //Debug.Log(aux);
        }
        //Debug.Log("Finished Creating HeatMap");
        return heatMap;
    }

    public void OnEnemyDoesDamage(int index, int damage)
    {
        damageDoneByEnemy[index]+=damage;
    }

    public void OnDeath()
    {
        if (actualRoomInfo.roomId != -1)
        {
            actualRoomInfo.playerFinalHealth = 0;
            actualRoomInfo.timeToExit = System.Convert.ToInt32(stopWatch.ElapsedMilliseconds) - actualRoomInfo.timeToExit;
            combatInfoList.Add(actualRoomInfo);
        }
        timesPlayerDied++;
    }

    public void OnRetry()
    {
        OnMapComplete();
    }
}
