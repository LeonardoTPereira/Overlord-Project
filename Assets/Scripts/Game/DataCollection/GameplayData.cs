using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Game.LevelManager.DungeonLoader;
using UnityEngine;

namespace Game.DataCollection
{
    public class GameplayData : MonoBehaviour
    {
        private const string Csv = ".csv";
        private const string PostDataURL = "http://damicore.icmc.usp.br/pag/data/upload.php?";
        private static int POST_QUESTIONS = 12;
        private static int NUMBER_OF_ENEMIES = 210;
        
        public static GameplayData instance = null;
        
        [SerializeField]
        private string profileString;
        [SerializeField]
        private string heatMapString;
        [SerializeField]
        private string levelProfileString;
        [SerializeField]
        private string detailedLevelProfileString;

        [SerializeField] private string sessionUID;

        // Auxiliary variables
        private List<string> playedLevels = new List<string>();
        private int attempts = 0;
        private bool pretestAnswered = false;
        private int curBatchId;
        protected int difficultyLevel; // TODO SET IT WITH THE NARRATIVE JSON
        
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
        
        //TODO receive event to send profile to server
        

        //File name: BatchId, MapId, SessionUID
        //Player profile: N Visited Rooms, N Unique Visited Rooms, N Keys Taken, N Keys Used, Form Answer 1, Form Answer 2,Form Answer 3
        private void SendProfileToServer(Map currentMap)
        {
            //StartCoroutine(PostData("Map" + levelID, profileString, heatMapString, levelProfileString, detailedLevelProfileString)); //TODO: verificar corretamente como serão salvos os arquivos
            //SaveToLocalFile("Map" + levelID, profileString, heatMapString, levelProfileString, detailedLevelProfileString);
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
                using (StreamWriter writer = new StreamWriter(target + "-Player" + Csv, true, Encoding.UTF8))
                {
                    writer.Write(stringData);
                    writer.Flush();
                    writer.Close();
                }
                pretestAnswered = true;
            }

            using (StreamWriter writer = new StreamWriter(target + "-Heatmap" + Csv, true, Encoding.UTF8))
            {
                writer.Write(heatMapData);
                writer.Flush();
                writer.Close();
            }

            using (StreamWriter writer = new StreamWriter(target + "-Level" + Csv, true, Encoding.UTF8))
            {
                writer.Write(levelData);
                writer.Flush();
                writer.Close();
            }

            using (StreamWriter writer = new StreamWriter(target + "-Detailed" + Csv, true, Encoding.UTF8))
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
            string post_url = PostDataURL;
            WWWForm form = new WWWForm();
            form.AddField("name", sessionUID);
            form.AddBinaryData("data", data, name + "-Player" + Csv, "text/csv");
            form.AddBinaryData("heatmap", heatMapBinary, name + "-Heatmap" + Csv, "text/csv");
            form.AddBinaryData("level", levelBinary, name + "-Level" + Csv, "text/csv");
            form.AddBinaryData("detailed", levelDetailedBinary, name + "-Detailed" + Csv, "text/csv");

            // Post the URL to the site and create a download object to get the result.
            WWW data_post = new WWW(post_url, form);
            yield return data_post; // Wait until the download is done

            if (data_post.error != null)
            {
                print("There was an error saving data: " + data_post.error);
            }
        }
    }
}