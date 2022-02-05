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
            health.text = "Vida: " + GameplayData.instance.actualRoomInfo.PlayerFinalHealth + "/10";
            combo.text = "Maior Combo: " + maxCombo;
            treasure.text = "Ouro: " + GameplayData.instance.TreasureCollected + "/" + GameManagerSingleton.Instance.maxTreasure;
            rooms.text = "Salas Exploradas: " + GameplayData.instance.visitedRooms.Distinct().Count() + "/" + GameManagerSingleton.Instance.maxRooms;
        }


    }
}
