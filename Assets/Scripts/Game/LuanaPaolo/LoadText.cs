using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadText : MonoBehaviour
{
    [System.Serializable]
    public class quest{
        public int tipo, n1, n2, n3, parent, c1, c2;
    }

    [System.Serializable]
    public class quests{
        public quest[] graph;
    }

    public TextAsset jsonFile;
    public TextMeshProUGUI missionSum;

    // Start is called before the first frame update
    void Start()
    {
        Quests questAux = JsonUtility.FromJson<Quests>(jsonFile.text);
        missionSum.text = "Mission Sum:\n";
        //quest q = questAux; 
 
        foreach(quest q in questAux.graph){
            switch(q.tipo){
                case 0: missionSum.text += "Find a treasure of type " + q.n1 + ".\n"; break;
                case 1: missionSum.text += "Find the dungeon's secret room.\n"; break;
                case 2: missionSum.text += "Kill " + q.n1 + " enemies of type 1, " + q.n2 + " enemies of type 2 and " + q.n3 + " of type 3.\n"; break;
                case 3: missionSum.text += "Get " + q.n1 + " itens.\n"; break;
                case 4: missionSum.text += "Explore the dungeon's rooms.\n"; break;
                case 5: missionSum.text += "Kill " + q.n1 + " enemies to get the same amount of itens dropped by them.\n"; break;
                case 6: missionSum.text += "Tal to the NPC " + q.n1 + ".\n"; break;
            }
        }

        Debug.Log(missionSum.text);
    }
}
