using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Game.GameManager;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class DungeonLoader : MonoBehaviour
{
    [SerializeField]
    private bool isArena;
    
    [field: MustBeAssigned, SerializeField]
    public DungeonFileSo SelectedDungeon { get; set; }
    [field: MustBeAssigned, SerializeField]
    public QuestLine LevelQuestLine { get; set; }

    [SerializeField] 
    private SceneReference dungeonScene;
    
    public static event LevelLoadEvent LoadLevelEventHandler;

    private void Start()
    {
        if (!dungeonScene.IsAssigned)
        {
            dungeonScene.SceneName = null;
            dungeonScene.SceneName = "LevelWithEnemies";
        }
        if (isArena)
        {
            LoadLevel();
        }
    }

    /// Load the level from the given filename.
    public void LoadLevel()
    {
        LoadLevelEventHandler?.Invoke(this, new LevelLoadEventArgs(SelectedDungeon, LevelQuestLine));
        SceneManager.LoadScene(dungeonScene.SceneName);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadLevel();
        }
    }
}
