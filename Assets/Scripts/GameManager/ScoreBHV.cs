using System.Collections;
using System.Collections.Generic;
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
        if (PlayerProfile.instance.actualCombo > PlayerProfile.instance.maxCombo)
            maxCombo = PlayerProfile.instance.actualCombo;
        else
            maxCombo = PlayerProfile.instance.maxCombo;
        health.text = "Vida: " + PlayerProfile.instance.actualRoomInfo.playerFinalHealth + "/10";
        combo.text = "Maior Combo: " + maxCombo;
        treasure.text = "Ouro: " + PlayerProfile.instance.treasureCollected + "/"+ GameManager.instance.maxTreasure;
        rooms.text = "Salas Exploradas: " + PlayerProfile.instance.visitedRooms.Distinct().Count() +"/" +GameManager.instance.maxRooms;
    }


}
