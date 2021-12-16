﻿using System;
using System.ComponentModel;
using UnityEngine;

namespace Game.LevelManager
{
    [Serializable]
    public class Coordinates
    {
        [SerializeField]
        private int x, y;
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
        [DefaultValue(-1)]
        public int X { get => x; set => x = value; }
        [DefaultValue(-1)]
        public int Y { get => y; set => y = value; }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            Coordinates coordinates = (Coordinates)obj;
            return coordinates.x == x && coordinates.y == y;
        }

        public override string ToString()
        {
            return $"Coordinates X = {X}, Y = {Y}";
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }
    }
}