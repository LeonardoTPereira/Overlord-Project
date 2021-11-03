using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator;
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
<<<<<<< HEAD
        public static event LevelLoadEvent loadLevelEventHandler;
=======
        //LoadDataForExperiment();
        /*DungeonFileSO dungeonFileSO;

        dungeonFileSO = ScriptableObject.CreateInstance<DungeonFileSO>();
        dungeonFileSO.Init(dungeonFile);
        Debug.Log(jsonContent);
#if UNITY_EDITOR
        AssetDatabase.CreateAsset(dungeonFileSO, "Assets/Resources/" + levelFileName + ".asset");
#endif*/
        DungeonTester.LoadLevel(this, dungeonFileSO);
    }

    /// Load the level from the given dungeon scriptable object.
    public static void LoadLevel(object from, DungeonFileSO dungeonFileSO)
    {
        loadLevelEventHandler(from, new LevelLoadEventArgs(dungeonFileSO));
        SceneManager.LoadScene("LevelWithEnemies");
    }
>>>>>>> Arena

        private NarrativeFilesSO narrativeFiles;
        public DungeonFileSO dungeonFileSO;

        private void Start()
        {
            loadLevelEventHandler(this, new LevelLoadEventArgs(dungeonFileSO));
            SceneManager.LoadScene("LevelWithEnemies");
        }
    }
}
