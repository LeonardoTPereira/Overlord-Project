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
    [SerializeField] private PlayerShip _player;

    private int _lifeAmount;
    private int _bombAmount;
    private int _enemiesDefeated;
    private int _playerPower;

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
        _lifeAmount = _player.maxLives;
        _bombAmount = _player.maxBombs;
        _playerPower = _player.damagePerBullet;
        _enemiesDefeated = -1;
        SumLife(0);
        SumPower(0);
        SumBomb(0);
        SumEnemyDefeated();
    }

    public void SumLife(int amount)
    {
        _lifeAmount += amount;
        _lifeAmountText.text = _lifeAmount.ToString();
    }

    public void SumBomb(int amount)
    {
        _bombAmount += amount;
        _bombAmountText.text = _bombAmount.ToString();
    }

    public void SumPower(int amount)
    {
        _playerPower += amount;
        _playerPowerText.text = _playerPower.ToString();
    }

    public void SumEnemyDefeated()
    {
        _enemiesDefeated ++;
        _enemiesDefeatedText.text = _enemiesDefeated.ToString();        
    }
}
