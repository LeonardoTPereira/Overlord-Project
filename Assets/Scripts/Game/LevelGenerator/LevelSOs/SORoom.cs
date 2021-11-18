using Game.LevelManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using ScriptableObjects;
using UnityEngine;

[Serializable]
public class SORoom
{
    public Coordinates coordinates;
    public string type;
    public List<int> keys;
    public List<int> locks;

    public int treasures = -1;
    public int npcs = -1;
    public int totalEnemies;

    public int TotalEnemies
    {
        get => totalEnemies;
        set => totalEnemies = value;
    }

    [DefaultValue(-1)]
    public int Treasures { get => treasures; set => treasures = value; }
    [DefaultValue(-1)]
    public int Npcs { get => npcs; set => npcs = value; }
}
