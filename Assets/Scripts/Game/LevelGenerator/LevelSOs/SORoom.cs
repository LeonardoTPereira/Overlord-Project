using Game.LevelManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class SORoom
{
    public Coordinates coordinates;
    public string type;
    public List<int> keys;
    public List<int> locks;
    private int enemies = -1;
    private int treasures = -1;
    private int enemyType = -1;
    private int npcs = -1;

    [DefaultValue(-1)]
    public int Enemies { get => enemies; set => enemies = value; }
    [DefaultValue(-1)]
    public int Treasures { get => treasures; set => treasures = value; }
    [DefaultValue(-1)]
    public int EnemiesType { get => enemyType; set => enemyType = value; }
    [DefaultValue(-1)]
    public int Npcs { get => npcs; set => npcs = value; }
}