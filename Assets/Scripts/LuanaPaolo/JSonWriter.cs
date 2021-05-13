using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSonWriter
{
    class parametersDungeon{
        public
            int size = 0, linearity = 0, nKeys = 0;
    };

    class parametersMonsters{
        public
            int nEnemies = 0, p1 = 0, p2 = 0, p3 = 0, f1 = 0, f2 = 0, f3 = 0;
    };

    public void writeJSon(List<Quest> graph){
        parametersDungeon pD = new parametersDungeon();
        parametersMonsters pM = new parametersMonsters();

        string outString = " ";

        for(int i = 0; i < graph.Count; i++) outString += JsonUtility.ToJson(graph[i]) + "\n";

        File.WriteAllText(Application.dataPath + "/narrative.txt", outString);

        conversorDungeon(pD, graph);

        outString = JsonUtility.ToJson(pD) + "\n";

        File.WriteAllText(Application.dataPath + "/dungeonGenerator.txt", outString);

        conversorMonsters(pM, graph);

        outString = JsonUtility.ToJson(pM) + "\n";

        File.WriteAllText(Application.dataPath + "/enemyGenerator.txt", outString);
    }

    private void conversorDungeon(parametersDungeon pD, List<Quest> graph){
        for(int i = 0; i < graph.Count; i++){
            if(graph[i].tipo == 1 || graph[i].tipo == 3 || graph[i].tipo == 4) pD.size++;
            if(graph[i].n1 == 0 || graph[i].n1 == 1 || graph[i].n1 == 4) pD.linearity++;
        }

        if(pD.size < 3) pD.size = 0;
        else if(pD.size >= 3 && pD.size < 7) pD.size = 1;
        else pD.size = 2;

        if(pD.linearity < 3) pD.linearity = 100;
        else if(pD.linearity >= 3 && pD.linearity < 7) pD.linearity = 101;
        else pD.linearity = 102;

        if(pD.nKeys < 3) pD.nKeys = 200;
        else if(pD.nKeys >= 3 && pD.nKeys < 7) pD.nKeys = 201;
        else pD.nKeys = 202;
    }

    private void conversorMonsters(parametersMonsters pM, List<Quest> graph){
        for(int i = 0; i < graph.Count; i++){
            if(graph[i].tipo == 2 || graph[i].tipo == 5){
                pM.nEnemies += 2;
                pM.p1 = graph[i].n1;
                pM.p2 = graph[i].n2;
                pM.p3 = graph[i].n3;
            }
        }
    }
}
