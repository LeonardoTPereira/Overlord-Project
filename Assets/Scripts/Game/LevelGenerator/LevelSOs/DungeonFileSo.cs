using Game.LevelManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable, CreateAssetMenu]
public class DungeonFileSo : ScriptableObject
{
    public Dimensions dimensions;
    public List<SORoom> rooms;

    private int _currentIndex = 0;

    public void ResetIndex()
    {
        _currentIndex = 0;
    }

    public DungeonPart GetNextPart()
    {
        if (_currentIndex < rooms.Count)
            return DungeonPartFactory.CreateDungeonPartFromDungeonFileSO(rooms[_currentIndex++]);
        return null;
    }
}