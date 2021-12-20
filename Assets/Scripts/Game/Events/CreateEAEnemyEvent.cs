using System;
using Game.EnemyGenerator;

namespace Game.Events
{
    public delegate void CreateEAEnemyEvent(object sender, CreateEAEnemyEventArgs e);

    public class CreateEAEnemyEventArgs : EventArgs
    {
        private DifficultyEnum difficulty;

        public CreateEAEnemyEventArgs(DifficultyEnum difficulty)
        {
            Difficulty = difficulty;
        }

        public DifficultyEnum Difficulty
        {
            get => difficulty;
            set => difficulty = value;
        }
    }
}
