using System.Linq;
using Game.DataCollection;
using TMPro;
using UnityEngine;

namespace Game.GameManager
{
    public class ScoreBHV : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI health, combo, treasure, rooms;

        private void OnEnable()
        {
            var maxCombo = GameplayData.instance.actualCombo > GameplayData.instance.MAXCombo 
                ? GameplayData.instance.actualCombo 
                : GameplayData.instance.MAXCombo;
            health.text = "Health: " + GameplayData.instance.actualRoomInfo.PlayerFinalHealth + "/10";
            combo.text = "Max Combo: " + maxCombo;
            treasure.text = "Treasure: " + GameplayData.instance.TreasureCollected + "/" + GameManagerSingleton.Instance.maxTreasure;
            rooms.text = "Explored Rooms: " + GameplayData.instance.visitedRooms.Distinct().Count() + "/" + GameManagerSingleton.Instance.maxRooms;
        }


    }
}
