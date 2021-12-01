using System.Linq;
using Game.DataCollection;
using Game.GameManager;
using TMPro;
using UnityEngine;

public class ScoreBHV : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health, combo, treasure, rooms;

    private void OnEnable()
    {
        int maxCombo;
        maxCombo = GameplayData.instance.actualCombo > GameplayData.instance.maxCombo 
            ? GameplayData.instance.actualCombo 
            : GameplayData.instance.maxCombo;
        health.text = "Vida: " + GameplayData.instance.actualRoomInfo.playerFinalHealth + "/10";
        combo.text = "Maior Combo: " + maxCombo;
        treasure.text = "Ouro: " + GameplayData.instance.treasureCollected + "/" + GameManagerSingleton.instance.maxTreasure;
        rooms.text = "Salas Exploradas: " + GameplayData.instance.visitedRooms.Distinct().Count() + "/" + GameManagerSingleton.instance.maxRooms;
    }


}
