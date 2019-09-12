using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerProfile : MonoBehaviour {

    public static PlayerProfile instance = null;

    private const string PostDataURL = "http://jogos.icmc.usp.br/pag/data/upload.php?";
    private int attemptNumber = 1; //TODO: entender o por quê desse int

    [SerializeField]
    private string sessionUID;
    [SerializeField]
    private string profileString, heatMapString;

    [SerializeField]
    private int mapCount = 0;
    [SerializeField]
    private int curMapId, curBatchId;

    [SerializeField]
    private List<Vector2Int> visitedRooms = new List<Vector2Int>();
    [SerializeField]
    private int mapVisitedCount = 0;
    [SerializeField]
    private int mapVisitedCountUnique = 0;
    [SerializeField]
    private int keysTaken = 0;
    [SerializeField]
    private int keysUsed = 0;
    [SerializeField]
    private List<int> formAnswers = new List<int>();
    [SerializeField]
    private int secondsToFinish = 0;
    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
    [SerializeField]
    private int[,] heatMap;
    


    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
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
        sessionUID = Random.Range(0, 99).ToString("00");
        sessionUID += "_";
        sessionUID += dateTime;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Events
    //From DoorBHV
    public void OnRoomFailEnter(Vector2Int offset)
    {
        //Log
        //Mais métricas - organiza em TAD
    }

    //From DoorBHV
    public void OnRoomEnter (int x, int y)
    {
        //Log
        //Mais métricas - organiza em TAD
        heatMap[x / 2, y / 2]++;
        visitedRooms.Add(new Vector2Int(x, y));
    }

    //From DoorBHV
    public void OnRoomFailExit(Vector2Int offset)
    {
        //Log
        //Mais métricas - organiza em TAD
    }

    //From DoorBHV
    public void OnRoomExit(Vector2Int offset)
    {
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
    public void OnMapStart (int id, int batch, Room[,] rooms)
    {
        mapCount++;
        curMapId = id;
        curBatchId = batch;
        stopWatch.Start();
        heatMap = CreateHeatMap(rooms);
        
        
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
        //Save to remote file
        SendProfileToServer();
        //Reset all values
        visitedRooms.Clear();
        formAnswers.Clear();
        keysTaken = 0;
        keysUsed = 0;
        profileString = "";
    }

    //From KeyBHV
    public void OnGetKey (int id)
    {
        //Log
        keysTaken++;
        //Mais métricas - organiza em TAD
    }

    //From FormBHV
    public void OnFormAnswered(int answer)
    {
        //Log
        formAnswers.Add(answer);
    }

    private void WrapProfileToString ()
    {
        profileString = "";
        profileString += mapVisitedCount + "," + mapVisitedCountUnique + "," + keysTaken + "," + keysUsed + ","+ secondsToFinish;
        foreach(int answer in formAnswers)
        {
            profileString += "," + answer;
        }
    }

    private void WrapHeatMapToString()
    {
        heatMapString = "";
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
        StartCoroutine(PostData("Batch"+curBatchId.ToString() +"Map" + curMapId.ToString(), profileString, heatMapString)); //TODO: verificar corretamente como serão salvos os arquivos
    }

    IEnumerator PostData(string name, string stringData, string heatMapData)
    {
        stringData = sessionUID + "," + stringData;
        byte[] data = System.Text.Encoding.UTF8.GetBytes(stringData);
        byte[] heatMapBinary = System.Text.Encoding.UTF8.GetBytes(heatMapData);
        //This connects to a server side php script that will write the data
        //string post_url = postDataURL + "name=" + WWW.EscapeURL(name) + "&data=" + data ;
        string post_url = PostDataURL;
        Debug.Log("LogName:"+name);
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddBinaryData("data", data, name + "_" + attemptNumber + ".txt", "text/plain");
        form.AddBinaryData("heatmap", heatMapBinary, "HM"+name + "_" + attemptNumber + ".txt", "text/plain");


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
        Debug.Log("Finished Creating HeatMap");
        return heatMap;
    }
}
