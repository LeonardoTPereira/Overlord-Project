using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSonWriter
{
    [System.Serializable]
    public class parametersDungeon{
        public
            int size = 0, linearity = 0, nKeys = 0, nEnemies = -1;
        public override string ToString()
        {
            return "Size=" + size + "_Keys=" + nKeys + "_lin=" + linearity + "_NEnemies=" + nEnemies;
        }
    }
    [System.Serializable]
    public class parametersMonsters{
        public
            int nEnemies = 0, p1 = 0, p2 = 0, p3 = 0, f1 = 0, f2 = 0, f3 = 0;
        public override string ToString()
        {
            return "p1=" + p1 + "_p2=" + p2 + "_p3=" + p3 + "_f1=" + f1+ "_f2=" + f2 + "_f3=" + f3;
        }
    };

    public void writeJSon(List<Quest> graph)
    {
        parametersDungeon pD = new parametersDungeon();
        parametersMonsters pM = new parametersMonsters();

        Directory.CreateDirectory(Application.dataPath + "\\Resources\\NarrativeJSon" + graph[0].ToString());
        Directory.CreateDirectory(Application.dataPath + "\\Resources\\NarrativeJSon" + graph[0].ToString() + "\\Dungeon");
        Directory.CreateDirectory(Application.dataPath + "\\Resources\\NarrativeJSon" + graph[0].ToString() + "\\Enemy");

        string outString = " ";

        for (int i = 0; i < graph.Count; i++) 
            outString += JsonUtility.ToJson(graph[i]) + "\n";

        File.WriteAllText(Application.dataPath + "/Resources/NarrativeJSon"+graph[0].ToString()+"/narrative.json", outString);

        conversorDungeon(pD, graph);

        outString = JsonUtility.ToJson(pD) + "\n";

        File.WriteAllText(Application.dataPath + "/Resources/NarrativeJSon"+graph[0].ToString()+"/Dungeon/"+pD.ToString()+".json", outString);

        conversorMonsters(pM, graph);

        outString = JsonUtility.ToJson(pM) + "\n";

        File.WriteAllText(Application.dataPath + "/Resources/NarrativeJSon"+graph[0].ToString()+"/Enemy/"+pM.ToString()+".json", outString);
    }

    private void conversorDungeon(parametersDungeon pD, List<Quest> graph)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            if (graph[i].tipo == 1 || graph[i].tipo == 3 || graph[i].tipo == 4 || graph[i].tipo == 6) pD.size++;
            if (graph[i].n1 == 0 || graph[i].n1 == 1 || graph[i].n1 == 4) pD.linearity++;
        }

        if (pD.size < 3) pD.size = (int)DungeonSize.VerySmall;
        else if (pD.size >= 3 && pD.size < 7) pD.size = (int)DungeonSize.Medium;
        else pD.size = (int)DungeonSize.VeryLarge;

        if (pD.linearity < 3) pD.linearity = (int)DungeonLinearity.VeryLinear;
        else if (pD.linearity >= 3 && pD.linearity < 7) pD.linearity = (int)DungeonLinearity.Medium;
        else pD.linearity = (int)DungeonLinearity.VeryBranched;

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
                pM.p1 = graph[i].n1;
                pM.p2 = graph[i].n2;
                pM.p3 = graph[i].n3;

                if (pM.p1 >= 5) pM.f1 = Random.Range(10, 21);
                else pM.f1 = Random.Range(1, 10);

                if (pM.p2 >= 5) pM.f2 = Random.Range(5, 21);
                else pM.f2 = Random.Range(1, 5);

                if (pM.p3 >= 5) pM.f3 = Random.Range(1, 10);
                else pM.f3 = Random.Range(10, 21);
            }
        }
    }
}
