using System.Collections.Generic;
using System.Linq;
using Game.Maestro;
using MyBox;
using Newtonsoft.Json;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameManager
{
    public class DungeonTester : MonoBehaviour
    {
        public static event LevelLoadEvent loadLevelEventHandler;

        private NarrativeFilesSO narrativeFiles;
        public DungeonFileSO dungeonFileSO;

        private void Start()
        {
            loadLevelEventHandler(this, new LevelLoadEventArgs(dungeonFileSO));
            SceneManager.LoadScene("LevelWithEnemies");
        }
    }
}
