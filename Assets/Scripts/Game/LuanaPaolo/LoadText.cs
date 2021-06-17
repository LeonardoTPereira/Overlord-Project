using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadText : MonoBehaviour
{
    [System.Serializable]
    public class Quest{
        public int tipo, n1, n2, n3, parent, c1, c2;
    }

    [System.Serializable]
    public class Quests{
        public Quest[] graph;
    }

    public TextAsset jsonFile;
    public TextMeshProUGUI missionSum;
    public QuestManager manager;

    // Start is called before the first frame update
    void Start()
    {
        /*quests questAux = JsonUtility.FromJson<quests>(jsonFile.text);
        missionSum.text = "Mission Sum:\n";
 
        foreach(quest q in questAux.graph){
            switch(q.tipo){
                case 0: missionSum.text += "Find a treasure of type " + q.n1 + ".\n"; manager.treasure = true; break;
                case 1: missionSum.text += "Find the dungeon's secret room.\n"; manager.secretRoom = true; break;
                case 2: missionSum.text += "Kill " + q.n1 + " enemies of type 1, " + q.n2 + " enemies of type 2 and " + q.n3 + " of type 3.\n"; manager.totalEnemies += q.n1 + q.n2 + q.n3; break;
                case 3: missionSum.text += "Get " + q.n1 + " itens.\n"; manager.totalItens += q.n1; break;
                case 4: missionSum.text += "Explore the dungeon's rooms.\n"; break;
                case 5: missionSum.text += "Kill " + q.n1 + " enemies to get the same amount of itens dropped by them.\n"; manager.totalEnemies += q.n1; break;
                case 6: missionSum.text += "Talk to the NPC " + q.n1 + ".\n"; manager.totalNpcs += q.n1; break;
            }
        }

        Debug.Log(missionSum.text);*/
    }

    private void clearManager(){
        manager.totalEnemies = 0;
        manager.totalItens = 0;
        manager.totalNpcs = 0;
        manager.secretRoom = false;
        manager.treasure = false;
    }

    public void CloseDialogue()
    {
        gameObject.SetActive(false);
    }
}
