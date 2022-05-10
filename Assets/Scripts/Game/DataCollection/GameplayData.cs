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
        private const string Json = ".json";
        private const string PostDataURL = "http://damicore.icmc.usp.br/pag/data/upload.php?";



        //TODO receive event to send profile to server
        

        //File name: BatchId, MapId, SessionUID
        //Player profile: N Visited Rooms, N Unique Visited Rooms, N Keys Taken, N Keys Used, Form Answer 1, Form Answer 2,Form Answer 3
        private void SendProfileToServer(Map currentMap)
        {
            //StartCoroutine(PostData("Map" + levelID, profileString, heatMapString, levelProfileString, detailedLevelProfileString)); //TODO: verificar corretamente como serão salvos os arquivos
            //SaveToLocalFile("Map" + levelID, profileString, heatMapString, levelProfileString, detailedLevelProfileString);
            //string UploadFilePath = GameplayData.instance.sessionUID;
        }

        /*private IEnumerator PostData(string name, string stringData, string heatMapData, string levelData, string levelDetailedData)
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
        }*/
    }
}