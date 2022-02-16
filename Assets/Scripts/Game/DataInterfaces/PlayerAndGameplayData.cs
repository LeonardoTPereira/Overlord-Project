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
            _totalVisits = totalVisits;
            _totalRooms = totalRooms;
            _numberOfVisitedRooms = numberOfVisitedRooms;
            _collectedKeys = collectedKeys;
            _totalKeys = totalKeys;
            _openedLocks = openedLocks;
            _totalLocks = totalLocks;
            _collectedTreasures = collectedTreasures;
            _totalTreasures = totalTreasures;
            _enemiesDefeated = enemiesDefeated;
            _totalEnemies = totalEnemies;

            PreTestAnswers.AddRange( preTestAnswers );
            PostTestAnswers.AddRange( postTestAnswers );
        }
        
        private int _hasDied;
        private int _hasFinished;
        private int _totalVisits;
        private int _totalRooms;
        private int _numberOfVisitedRooms;
        private int _collectedKeys;
        private int _totalKeys;
        private int _openedLocks;
        private int _totalLocks;
        private int _collectedTreasures;
        private int _totalTreasures;
        private int _enemiesDefeated;
        private int _totalEnemies;
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
        public int TotalVisits
        {
            get => _totalVisits;
        }
        public int TotalRooms
        {
            get => _totalRooms;
        }
        public int NumberOfVisitedRooms
        {
            get => _numberOfVisitedRooms;
        }
        public int CollectedKeys
        {
            get => _collectedKeys;
        }
        public int TotalKeys
        {
            get => _totalKeys;
        }
        public int OpenedLocks
        {
            get => _openedLocks;
        }
        public int TotalLocks
        {
            get => _totalLocks;
        }
        public int CollectedTreasures
        {
            get => _collectedTreasures;
        }
        public int TotalTreasures
        {
            get => _totalTreasures;
        }
        public int EnemiesDefeated
        {
            get => _enemiesDefeated;
        }
        public int TotalEnemies
        {
            get => _totalEnemies;
        }
    }
}