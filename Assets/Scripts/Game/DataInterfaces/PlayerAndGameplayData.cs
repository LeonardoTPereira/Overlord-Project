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
            int totalEnemies, int playerLostHealth, int maxCombo, int player_id, int map_id, int chosen_weapon, int elapsed_time, bool has_finishedLvl, bool has_diedLvl,total_keys,collected_keys,total_locks,opened_locks,total_rooms,number_of_visited_rooms,total_visits,number_of_over_visited_rooms,player_initial_health,player_final_health,player_lost_health,number_of_enemies,number_of_killed_enemies,number_of_npcs,number_of_interacted_npcs,total_treasures,collected_treasures,max_combo, List<int> postQuestionLvl, int arq)
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

            PreTestAnswers = new List<int>();
            PreTestAnswers.AddRange( preTestAnswers );
            PostTestAnswers = new List<int>();
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