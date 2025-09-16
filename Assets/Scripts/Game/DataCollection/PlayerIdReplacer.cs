using UnityEngine;
using System;
using TMPro;
using Game.DataCollection;

public class PlayerIdReplacer : MonoBehaviour
{
    [SerializeField] private PlayerDataController playerDataController;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        text.text = text.text.Replace("{id}", playerDataController.CurrentPlayer.SerializedData.PlayerId.ToString());
    }
}