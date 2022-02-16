using System;
using System.Collections.Generic;

namespace Game.DataInterfaces
{
    public class PlayerAndGameplayData
    {
        public PlayerAndGameplayData(List<int> preTestAnswers, List<int> postTestAnswers,
            bool hasDied, bool hasFinished, int totalVisits, int totalRooms, 
            int numberOfVisitedRooms, int collectedKeys, int totalKeys, int openedLocks, 
            int totalLocks, int collectedTreasures, int totalTreasures, int enemiesDefeated,
            int totalEnemies)
        {
            _hasDied = Convert.ToInt32(hasDied);
            _hasFinished = Convert.ToInt32(hasFinished);
            //TODO convert other data
        }
        
        private int _hasDied;
        private int _hasFinished;
        public List<int> PreTestAnswers { get; set; }
        public List<int> PostTestAnswers { get; set; }

        public int HasDied
        {
            get => _hasDied;
        }
        public int HasFinished
        {
            get => _hasFinished;
        }
    }
}