using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI collectiblesText;
    private int _score = 0;
    private int _collectibles = 0;
    public static ScoreManager Instance { get; private set; }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        scoreText.text = "0";
        collectiblesText.text = "0";
    }

    public void AddScore(int points)
    {
        _score += points;
        scoreText.text = _score.ToString();
    }

    public void AddCollectible(int amount)
    {
        _collectibles += amount;
        collectiblesText.text = _collectibles.ToString();
    }

    public int GetCollectibles()
    {
        return _collectibles;
    }
}
