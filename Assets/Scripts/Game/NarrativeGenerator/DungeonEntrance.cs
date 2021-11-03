﻿using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Game.GameManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour
{
    public string nameScene;

    [field: SerializeField] public DungeonFileSO DungeonFileSo { get; set; }
    private GameManagerSingleton _gameManagerSingleton;
    
    public static event LevelLoadEvent loadLevelEventHandler;

    /// Load the level from the given filename.
    public static void LoadLevel(object from, string filename)
    {
        loadLevelEventHandler(from, new LevelLoadEventArgs(filename));
        SceneManager.LoadScene("LevelWithEnemies");
    }

    void Start()
    {
        _gameManagerSingleton = FindObjectOfType<GameManagerSingleton>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
<<<<<<< HEAD:Assets/Scripts/Game/NarrativeGenerator/DungeonEntrance.cs
            loadLevelEventHandler(this, new LevelLoadEventArgs(DungeonFileSo));
            SceneManager.LoadScene("LevelWithEnemies");
=======
            LoadLevel(this, LevelFileName);
>>>>>>> Arena:Assets/Scripts/Game/LuanaPaolo/DungeonEntrance.cs
        }
    }
}
