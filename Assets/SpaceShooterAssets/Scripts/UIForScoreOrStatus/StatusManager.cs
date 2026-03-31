using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lifeAmountText;
    [SerializeField] private TextMeshProUGUI _bombAmountText;
    [SerializeField] private TextMeshProUGUI _enemiesDefeatedText;
    [SerializeField] private TextMeshProUGUI _playerPowerText;
    [SerializeField] private TextMeshProUGUI _completedQuestsText;
    [SerializeField] private PlayerShip _player;

    public int LifeAmount { get; set; }
    public int BombAmount { get; set; }
    public int EnemiesDefeated { get; set; }
    public int PlayerPower { get; set; }
    public int CompletedQuests { get; set; }
    public int VisitedRooms { get; set; }

    public static StatusManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        LifeAmount = _player.maxLives;
        BombAmount = _player.maxBombs;
        PlayerPower = _player.damagePerBullet;
        EnemiesDefeated = -1;
        SumLife(0);
        SumPower(0);
        SumBomb(0);
        SumEnemyDefeated();
    }

    public void SumLife(int amount)
    {
        FindObjectOfType<PlayerShip>().IncreaseLives(amount);
        LifeAmount += amount;
        _lifeAmountText.text = LifeAmount.ToString();
    }

    public void SumBomb(int amount)
    {
        FindObjectOfType<PlayerShip>().IncreaseBombs(amount);
        BombAmount += amount;
        _bombAmountText.text = BombAmount.ToString();
    }

    public void SumPower(int amount)
    {
        FindObjectOfType<PlayerShip>().IncreasePower(amount);
        PlayerPower += amount;
        _playerPowerText.text = PlayerPower.ToString();
    }

    public void SumEnemyDefeated()
    {
        EnemiesDefeated ++;
        _enemiesDefeatedText.text = EnemiesDefeated.ToString();        
    }

    public void AddCompletedQuest()
    {
        CompletedQuests++;
        _completedQuestsText.text = CompletedQuests.ToString();
    }

    public void RewardPlayer(int amount)
    {
        SumLife(amount);
        SumPower(amount);
    }
}
