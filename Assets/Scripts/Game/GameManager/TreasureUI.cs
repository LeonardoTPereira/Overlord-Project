using System;
using TMPro;
using UnityEngine;

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
        TreasureController.treasureCollectEvent += IncrementTreasure;
        GameManager.NewLevelLoadedEventHandler += ResetTreasure;
    }

    protected void OnDisable()
    {
        TreasureController.treasureCollectEvent -= IncrementTreasure;
        GameManager.NewLevelLoadedEventHandler -= ResetTreasure;
    }

    public void IncrementTreasure(object sender, TreasureCollectEventArgs eventArgs)
    {
        treasureAmount += eventArgs.Amount;
        treasureText.text = "x " + treasureAmount.ToString();
    }

    public void ResetTreasure(object sender, EventArgs eventArgs)
    {
        treasureAmount = 0;
        treasureText.text = "x " + treasureAmount.ToString();
    }

}
