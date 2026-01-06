using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int _score = 0;
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
    }

    public void AddScore(int points)
    {
        _score += points;
        scoreText.text = _score.ToString();
    }
}
