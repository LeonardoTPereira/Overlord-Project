﻿using System;
using Game.Events;
using TMPro;
using UnityEngine;

namespace Game.GameManager
{
    public class TreasureUI : MonoBehaviour
    {
        protected TextMeshProUGUI treasureText;
        // Start is called before the first frame update

        protected int treasureAmount;

        private void Awake()
        {
            treasureText = GetComponent<TextMeshProUGUI>();
            treasureAmount = 0;
        }

        protected void OnEnable()
        {
            TreasureController.TreasureCollectEventHandler += IncrementTreasure;
            GameManagerSingleton.NewLevelLoadedEventHandler += ResetTreasure;
        }

        protected void OnDisable()
        {
            TreasureController.TreasureCollectEventHandler -= IncrementTreasure;
            GameManagerSingleton.NewLevelLoadedEventHandler -= ResetTreasure;
        }

        public void IncrementTreasure(object sender, TreasureCollectEventArgs eventArgs)
        {
            treasureAmount += eventArgs.Amount;
            treasureText.text = "x " + treasureAmount;
        }

        public void ResetTreasure(object sender, EventArgs eventArgs)
        {
            treasureAmount = 0;
            treasureText.text = "x " + treasureAmount;
        }

    }
}
