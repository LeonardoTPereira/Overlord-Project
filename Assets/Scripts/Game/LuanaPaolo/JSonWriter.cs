using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class JSonWriter
{
    [System.Serializable]
    public class ParametersDungeon
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
            return DungeonLinearityConverter.ToFloat((DungeonLinearity)LinearityEnum);
        }
    }

    [System.Serializable]
    public class ParametersMonsters
    {
        private int nEnemies;
        private int percentageType1;
        private int percentageType2;
        private int percentageType3;
        private int frequencyType1;
        private int frequencyType2;
        private int frequencyType3;

        public int NEnemies { get => nEnemies; set => nEnemies = value; }
        public int PercentageType1 { get => percentageType1; set => percentageType1 = value; }
        public int PercentageType2 { get => percentageType2; set => percentageType2 = value; }
        public int PercentageType3 { get => percentageType3; set => percentageType3 = value; }
        public int FrequencyType1 { get => frequencyType1; set => frequencyType1 = value; }
        public int FrequencyType2 { get => frequencyType2; set => frequencyType2 = value; }
        public int FrequencyType3 { get => frequencyType3; set => frequencyType3 = value; }

        public ParametersMonsters()
        {
            NEnemies = 0;
            PercentageType1 = 0;
            PercentageType2 = 0;
            PercentageType3 = 0;
            FrequencyType1 = 0;
            FrequencyType2 = 0;
            FrequencyType3 = 0;
        }

        public override string ToString()
        {
            return "p1=" + PercentageType1 + "_p2=" + PercentageType2 + "_p3=" + PercentageType3 + "_f1=" + FrequencyType1+ "_f2=" + FrequencyType2 + "_f3=" + FrequencyType3;
        }
    }

    [System.Serializable]
    public class ParametersNpcs{
        private int numNpcs = 0;
        private List<Quest> quests;

        public int NumNpcs{get => numNpcs; set => numNpcs = value;}
        public List<Quest> Quests{get => quests; set => quests = value;}
    }

    [System.Serializable]
    public class ParametersItems{
        private int numItens = 0;

        public int NumItens{get => numItens; set => numItens = value;}
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
        outString = JsonConvert.SerializeObject(graph);
        string filename = template.Replace(CONTENT, narrativeFl);
        File.WriteAllText(filename, outString);

        // Get the dungeon parameters
        ParametersDungeon pD = new ParametersDungeon();
        conversorDungeon(pD, graph);
        // Convert the dungeon to JSON
        outString = JsonConvert.SerializeObject(pD) + '\n';
        // Write the dungeon JSON file
        filename = template.Replace(CONTENT, dungeonFd + sep + pD.ToString());
        File.WriteAllText(filename, outString);

        // Get the enemies parameters
        ParametersMonsters pM = new ParametersMonsters();
        conversorMonsters(pM, graph);
        // Convert the enemies to JSON
        outString = JsonConvert.SerializeObject(pM) + '\n';
        // Write the enemies JSON file
        filename = template.Replace(CONTENT, enemyFd + sep + pM.ToString());
        File.WriteAllText(filename, outString);

        ParametersNpcs pN = new ParametersNpcs();
        conversorNpcs(pN, graph);
        // Convert the enemies to JSON
        outString = JsonConvert.SerializeObject(pN) + '\n';
        // Write the enemies JSON file
        //filename = template.Replace(CONTENT, enemyFd + sep + pN.ToString());
        File.WriteAllText(filename, outString);

        ParametersItems pI = new ParametersItems();
        conversorItems(pI, graph);
        // Convert the enemies to JSON
        outString = JsonConvert.SerializeObject(pI) + '\n';
        // Write the enemies JSON file
        //filename = template.Replace(CONTENT, enemyFd + sep + pN.ToString());
        File.WriteAllText(filename, outString);
    }

    private void conversorDungeon(ParametersDungeon pD, List<Quest> graph)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            if (graph[i].Tipo == 1 || graph[i].Tipo == 3 || graph[i].Tipo == 4 || graph[i].Tipo == 6) pD.size++;
            if (graph[i].N1 == 0 || graph[i].N1 == 1 || graph[i].N1 == 4) pD.Linearity++;
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

    private void conversorMonsters(ParametersMonsters pM, List<Quest> graph)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            if (graph[i].Tipo == 2 || graph[i].Tipo == 5)
            {
                pM.NEnemies += 2;
                pM.PercentageType1 += graph[i].N1; //significados de n1, n2, n3 e tipo no script "Quest", favor verificar
                pM.PercentageType2 += graph[i].N2;
                pM.PercentageType3 += graph[i].N3;

                if (pM.PercentageType1 >= 5) pM.FrequencyType1 += Random.Range(10, 21);
                else pM.FrequencyType1 += Random.Range(1, 10);

                if (pM.PercentageType2 >= 5) pM.FrequencyType2 += Random.Range(5, 21);
                else pM.FrequencyType2 += Random.Range(1, 5);

                if (pM.PercentageType3 >= 5) pM.FrequencyType3 += Random.Range(1, 10);
                else pM.FrequencyType3 += Random.Range(10, 21);
            }
        }
    }

    private void conversorNpcs(ParametersNpcs pN, List<Quest> graph){
        for (int i = 0; i < graph.Count; i++){
            if(graph[i].tipo == 6){
                pN.NumNpcs++;
                if(i + 1 < graph.Count) pN.Quests.Add(graph[i + 1]);
                else{
                    Quest questAux = new Quest();
                    questAux.tipo = 10;
                    pN.Quests.Add(questAux);
                }
            }
        }
    }

    private void conversorItems(ParametersItems pI, List<Quest> graph){
        for (int i = 0; i < graph.Count; i++){
            if(graph[i].tipo == 3) pI.NumItens += graph[i].n1;
        }
    }
}
