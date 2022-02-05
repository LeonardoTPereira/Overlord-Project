using System;
using System.ComponentModel;
using UnityEngine;

namespace Game.LevelManager
{
    [Serializable]
    public class Coordinates
    {
        [SerializeField]
        private readonly int _x, _y;
        public Coordinates(int x, int y)
        {
            _x = x;
            _y = y;
        }
        [DefaultValue(-1)]
        public int X { get => _x; }
        [DefaultValue(-1)]
        public int Y { get => _y; }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            Coordinates coordinates = (Coordinates)obj;
            return coordinates.X == _x && coordinates.Y == _y;
        }

        public override string ToString()
        {
            return $"Coordinates X = {X}, Y = {Y}";
        }

        public override int GetHashCode()
        {
            return _x ^ _y;
        }
    }
}