using System;
using Game.EnemyGenerator;

namespace Game.Events
{
    public delegate void CreateEAEnemyEvent(object sender, CreateEAEnemyEventArgs e);

    public class CreateEAEnemyEventArgs : EventArgs
    {
        private DifficultyLevels difficulty;

        public CreateEAEnemyEventArgs(DifficultyLevels difficulty)
        {
            Difficulty = difficulty;
        }

        public DifficultyLevels Difficulty
        {
            get => difficulty;
            set => difficulty = value;
        }
    }
}
