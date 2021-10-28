using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using UnityEditor;
using UnityEngine;
using static Enums;
using static Util;

namespace Game.NarrativeGenerator
{
    /* TODO Save quest as asset
        "..." secretRoomQuest.SaveAsAsset(string assetName); "..."
     */
    public class JSonWriter
    {
        public void writeJSon(QuestList quests, PlayerProfileEnum playerProfile)
        {
            // Define the JSON file extension
            const string extension = ".json";

            // Build the target path
            string target = Application.dataPath;
            target += SEPARATOR_CHARACTER + "Resources";
            target += SEPARATOR_CHARACTER + "Experiment";
            target += SEPARATOR_CHARACTER + playerProfile.ToString();

            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            target += SEPARATOR_CHARACTER + "NarrativeJSon";
            target += quests.graph[0].ToString();

            // Define the filename template
            const string CONTENT = "CONTENT";
            string template = target + SEPARATOR_CHARACTER + CONTENT + extension;

            // Define the content folders' (Fd) and files' (Fl) names
            string narrativeFl = "narrative";
            string dungeonFd = "Dungeon";
            string enemyFd = "Enemy";
            string npcFd = "NPC";
            string itemFd = "Item";
            string generatedDungeonFolder = "Levels";

            // Create directories to save the generated contents

            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
                Directory.CreateDirectory(target + SEPARATOR_CHARACTER + dungeonFd);
                Directory.CreateDirectory(target + SEPARATOR_CHARACTER + enemyFd);
                Directory.CreateDirectory(target + SEPARATOR_CHARACTER + npcFd);
                Directory.CreateDirectory(target + SEPARATOR_CHARACTER + itemFd);
                Directory.CreateDirectory(target + SEPARATOR_CHARACTER + generatedDungeonFolder);
            }

            // Initialize output string
            string outString = "";
            // Convert the narrative to JSON
            ////for (int i = 0; i < graph.Count; i++) 
            ////outString += JsonUtility.ToJson(graph[i]) + '\n';
            // Write the narrative JSON file
            outString = JsonConvert.SerializeObject(quests);
            string filename = template.Replace(CONTENT, narrativeFl);


            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                streamWriter.Write(outString);
                streamWriter.Flush();
                streamWriter.Close();
            }


            // Get the dungeon parameters
            ParametersDungeon pD = new ParametersDungeon();
            // conversorDungeon(pD, quests);
            // Convert the dungeon to JSON
            outString = JsonConvert.SerializeObject(pD) + '\n';
            // Write the dungeon JSON file
            filename = template.Replace(CONTENT, dungeonFd + SEPARATOR_CHARACTER + pD.ToString());
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                streamWriter.Write(outString);
                streamWriter.Flush();
                streamWriter.Close();
            }

            // Get the enemies parameters
            EnemyParameters pM = new EnemyParameters();
            // pM.(pM, quests); //??
            // Convert the enemies to JSON
            outString = JsonConvert.SerializeObject(pM) + '\n';
            // Write the enemies JSON file
            filename = template.Replace(CONTENT, enemyFd + SEPARATOR_CHARACTER + pM.ToString());
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                streamWriter.Write(outString);
                streamWriter.Flush();
                streamWriter.Close();
            }

            ParametersNpcs pN = new ParametersNpcs();
            // pN.ConversorNpcs(quests);
            // Convert the enemies to JSON
            outString = JsonConvert.SerializeObject(pN) + '\n';
            // Write the enemies JSON file
            filename = template.Replace(CONTENT, npcFd + SEPARATOR_CHARACTER + pN.ToString());
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                streamWriter.Write(outString);
                streamWriter.Flush();
                streamWriter.Close();
            }

            ParametersItems pI = new ParametersItems();
            pI.ConversorItems(quests);
            // Convert the enemies to JSON
            outString = JsonConvert.SerializeObject(pI) + '\n';
            // Write the enemies JSON file
            filename = template.Replace(CONTENT, itemFd + SEPARATOR_CHARACTER + pN.ToString());
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                streamWriter.Write(outString);
                streamWriter.Flush();
                streamWriter.Close();
            }
            Resources.UnloadUnusedAssets();
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}
