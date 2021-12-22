﻿using System;
using Game.LevelManager;

namespace Game.Events
{
    public delegate void FinishMapEvent(object sender, FinishMapEventArgs e);
    public class FinishMapEventArgs : EventArgs
    {
        private Map map;

        public FinishMapEventArgs(Map map)
        {
            Map = map;
        }
        public Map Map { get => map; set => map = value; }
    }
}