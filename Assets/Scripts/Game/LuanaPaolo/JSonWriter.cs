using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class JSonWriter
{
    [System.Serializable]
    public class parametersDungeon
    {
        public int size = 0;
        public int nKeys = 0;
        public int nEnemies = -1;
        public float linearity;
        private int linearityMetric = 0;
        private int linearityEnum;

        public int Linearity
        {
            get => linearityMetric; 
            set => linearityMetric = value;
        }

        public int LinearityEnum
        {
            get => linearityEnum;
            set
            {
                linearityEnum = value;
                linearity = getLinearity();
            }
        }

        public override string ToString()
        {
            return "Size=" + size + "_Keys=" + nKeys + "_lin=" + getLinearity() + "_NEnemies=" + nEnemies;
        }

        public float getLinearity()
        {
            return DungeonLinearityConverter.ToFloat((DungeonLinearity)Linearity);
        }
    }

    [System.Serializable]
    public class parametersMonsters
    {
        public int nEnemies = 0;
        public int percentageType1 = 0;
        public int percentageType2 = 0;
        public int percentageType3 = 0;
        public int frequencyType1 = 0;
        public int frequencyType2 = 0;
        public int frequencyType3 = 0;

        public override string ToString()
        {
            return "p1=" + percentageType1 + "_p2=" + percentageType2 + "_p3=" + percentageType3 + "_f1=" + frequencyType1+ "_f2=" + frequencyType2 + "_f3=" + frequencyType3;
        }
    }

    public void writeJSon(List<Quest> graph)
    {
        // Get the directory separator
        char sep = Path.DirectorySeparatorChar;

        // Define the JSON file extension
        const string extension = ".json";

        // Build the target path
        string target = Application.dataPath;
        target += sep + "Resources";
        target += sep + "NarrativeJSon";
        target += graph[0].ToString();

        // Define the filename template
        const string CONTENT = "CONTENT";
        string template = target + sep + CONTENT + extension;

        // Define the content folders' (Fd) and files' (Fl) names
        string narrativeFl = "narrative";
        string dungeonFd = "Dungeon";
        string enemyFd = "Enemy";

        // Create directories to save the generated contents
        Directory.CreateDirectory(target);
        Directory.CreateDirectory(target + sep + dungeonFd);
        Directory.CreateDirectory(target + sep + enemyFd);

        // Initialize output string
        string outString = " ";
        // Convert the narrative to JSON
        ////for (int i = 0; i < graph.Count; i++) 
            ////outString += JsonUtility.ToJson(graph[i]) + '\n';
        // Write the narrative JSON file
        string filename = template.Replace(CONTENT, narrativeFl);
        ////File.WriteAllText(filename, outString);
        File.WriteAllText(filename, graph);

        // Get the dungeon parameters
        parametersDungeon pD = new parametersDungeon();
        conversorDungeon(pD, graph);
        // Convert the dungeon to JSON
        outString = JsonUtility.ToJson(pD) + '\n';
        // Write the dungeon JSON file
        filename = template.Replace(CONTENT, dungeonFd + sep + pD.ToString());
        File.WriteAllText(filename, outString);

        // Get the enemies parameters
        parametersMonsters pM = new parametersMonsters();
        conversorMonsters(pM, graph);
        // Convert the enemies to JSON
        outString = JsonUtility.ToJson(pM) + '\n';
        // Write the enemies JSON file
        filename = template.Replace(CONTENT, enemyFd + sep + pM.ToString());
        File.WriteAllText(filename, outString);
    }

    private void conversorDungeon(parametersDungeon pD, List<Quest> graph)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            if (graph[i].tipo == 1 || graph[i].tipo == 3 || graph[i].tipo == 4 || graph[i].tipo == 6) pD.size++;
            if (graph[i].n1 == 0 || graph[i].n1 == 1 || graph[i].n1 == 4) pD.Linearity++;
        }

        if (pD.size < 3) pD.size = (int)DungeonSize.VerySmall;
        else if (pD.size >= 3 && pD.size < 7) pD.size = (int)DungeonSize.Medium;
        else pD.size = (int)DungeonSize.VeryLarge;

        if (pD.Linearity < 3) pD.LinearityEnum = (int)DungeonLinearity.VeryLinear;
        else if (pD.Linearity >= 3 && pD.Linearity < 7) pD.LinearityEnum = (int)DungeonLinearity.Medium;
        else pD.LinearityEnum = (int)DungeonLinearity.VeryBranched;

        if(pD.nKeys < 3) pD.nKeys = (int)DungeonKeys.AFewKeys;
        else if(pD.nKeys >= 3 && pD.nKeys < 7) pD.nKeys = (int)DungeonKeys.SeveralKeys;
        else pD.nKeys = (int)DungeonKeys.LotsOfKeys;

        pD.nEnemies = Random.Range(1, 5);
    }

    private void conversorMonsters(parametersMonsters pM, List<Quest> graph)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            if (graph[i].tipo == 2 || graph[i].tipo == 5)
            {
                pM.nEnemies += 2;
                pM.percentageType1 = graph[i].n1; //significados de n1, n2, n3 e tipo no script "Quest", favor verificar
                pM.percentageType2 = graph[i].n2;
                pM.percentageType3 = graph[i].n3;

                if (pM.percentageType1 >= 5) pM.frequencyType1 = Random.Range(10, 21);
                else pM.frequencyType1 = Random.Range(1, 10);

                if (pM.percentageType2 >= 5) pM.frequencyType2 = Random.Range(5, 21);
                else pM.frequencyType2 = Random.Range(1, 5);

                if (pM.percentageType3 >= 5) pM.frequencyType3 = Random.Range(1, 10);
                else pM.frequencyType3 = Random.Range(10, 21);
            }
        }
    }
}
