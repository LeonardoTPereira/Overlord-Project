using System;
using ScriptableObjects;

namespace Game.Events
{
    public delegate void LevelSelectEvent(object sender, LevelSelectEventArgs e);
    public class LevelSelectEventArgs : EventArgs
    {
        private LevelConfigSO levelSO;

        public LevelSelectEventArgs(LevelConfigSO levelSO)
        {
            LevelSO = levelSO;
        }

        public LevelConfigSO LevelSO { get => levelSO; set => levelSO = value; }
    }
}