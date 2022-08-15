using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.Quests;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.LevelManager.DungeonLoader
{
    public class DungeonSceneLoader : MonoBehaviour
    {
        [SerializeField]
        private bool isArena;
    
        [field: SerializeField]
        public DungeonFileSo SelectedDungeon { get; set; }
        [field: SerializeField]
        public QuestLineList LevelQuestLines { get; set; }
        [field: SerializeField]
        public bool IsLastQuestLine { get; set; }

        [SerializeField] 
        private SceneReference dungeonScene;
    

        private void Start()
        {
            if (!dungeonScene.IsAssigned)
            {
                dungeonScene.SceneName = null;
                dungeonScene.SceneName = "LevelWithEnemies";
            }
            if (isArena)
            {
                LoadLevel();
            }
        }

        /// Load the level from the given filename.
        private void LoadLevel()
        {
            //TODO fix this to use the dungeon item SO from Level Selector and apply the data to it
            SceneManager.LoadScene(dungeonScene.SceneName);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("PlayerTrigger"))
            {
                LoadLevel();
            }
        }
    }
}
