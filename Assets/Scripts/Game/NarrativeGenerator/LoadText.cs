using UnityEngine;
using TMPro;

namespace Game.NarrativeGenerator
{
    public class LoadText : MonoBehaviour
    {
        public TextAsset jsonFile;
        public TextMeshProUGUI missionSum;
        public QuestManager manager;

        // Start is called before the first frame update
        //TODO fix this, if it may be used in the future
        /*void Start()
        {
            Quests questAux = JsonConvert.DeserializeObject<Quests>(jsonFile.text);
            missionSum.text = "Mission Sum:<br>";

            // foreach (QuestSO q in questAux.graph)
            // {
            //     switch (q.Tipo)
            //     {
            //         case 0: missionSum.text += "Find a treasure of type " + q.N1 + ".<br>"; manager.treasure = true; break;
            //         case 1: missionSum.text += "Find the dungeon's secret room.<br>"; manager.secretRoom = true; break;
            //         case 2: missionSum.text += "Kill " + q.N1 + " enemies of type 1,<br> " + q.N2 + " enemies of type 2 and <br>" + q.N3 + " of type 3.\n"; manager.totalEnemies += q.N1 + q.N2 + q.N3; break;
            //         case 3: missionSum.text += "Get " + q.N1 + " itens.<br>"; manager.totalItens += q.N1; break;
            //         case 4: missionSum.text += "Explore the dungeon's rooms.<br>"; break;
            //         case 5: missionSum.text += "Kill " + q.N1 + " enemies to get the same amount of itens dropped by them.<br>"; manager.totalEnemies += q.N1; break;
            //         case 6: missionSum.text += "Talk to the NPC " + q.N1 + ".<br>"; manager.totalNpcs += q.N1; break;
            //     }
            // }

            Debug.Log(missionSum.text);
        }*/

        private void clearManager()
        {
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
}
