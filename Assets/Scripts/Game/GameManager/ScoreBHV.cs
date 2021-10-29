using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreBHV : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI health, combo, treasure, rooms;

    private void OnEnable()
    {
        int maxCombo;
        if (GameplayData.instance.actualCombo > GameplayData.instance.maxCombo)
            maxCombo = GameplayData.instance.actualCombo;
        else
            maxCombo = GameplayData.instance.maxCombo;
        health.text = "Vida: " + GameplayData.instance.actualRoomInfo.playerFinalHealth + "/10";
        combo.text = "Maior Combo: " + maxCombo;
        treasure.text = "Ouro: " + GameplayData.instance.treasureCollected + "/" + GameManager.instance.maxTreasure;
        rooms.text = "Salas Exploradas: " + GameplayData.instance.visitedRooms.Distinct().Count() + "/" + GameManager.instance.maxRooms;
    }


}
