using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        TreasureController.collectTreasureEvent += IncrementTreasure;
        GameManager.newLevelLoadedEventHandler += ResetTreasure;
    }

    protected void OnDisable()
    {
        TreasureController.collectTreasureEvent -= IncrementTreasure;
        GameManager.newLevelLoadedEventHandler -= ResetTreasure;
    }

    public void IncrementTreasure(int amount)
    {
        treasureAmount += amount;
        treasureText.text = "x "+treasureAmount.ToString();
    }

    public void ResetTreasure(object sender, EventArgs eventArgs)
    {
        treasureAmount = 0;
        treasureText.text = "x " + treasureAmount.ToString();
    }

}
