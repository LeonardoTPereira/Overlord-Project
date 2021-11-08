﻿using System;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Game.GameManager;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class DungeonLoader : MonoBehaviour
{
    [SerializeField]
    private bool isArena;
    
    [field: MustBeAssigned, SerializeField]
    public DungeonFileSO SelectedDungeon { get; set; }

    [SerializeField] 
    private SceneReference dungeonScene;
    
    public static event LevelLoadEvent LoadLevelEventHandler;

    private void Awake()
    {
        dungeonScene ??= new SceneReference
        {
            Scene = null,
            SceneName = "LevelWithEnemies"
        };
    }

    private void Start()
    {
        if (isArena)
        {
            LoadLevel();
        }
    }

    /// Load the level from the given filename.
    public void LoadLevel()
    {
        LoadLevelEventHandler?.Invoke(this, new LevelLoadEventArgs(SelectedDungeon));
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