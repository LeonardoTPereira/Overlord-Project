using System.Collections.Generic;
using System.IO;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "QuestLine", menuName = "Overlord-Project/QuestLine", order = 0)]
    public class QuestLine : ScriptableObject
    {
        public List<QuestSO> graph;
        [SerializeField] private List<DungeonFileSO> _dungeonFileSos;
        [SerializeField] private QuestDungeonsParameters _dungeonParametersForQuestLine;
        [SerializeField] private QuestEnemiesParameters _enemyParametersForQuestLine;
        [SerializeField] private QuestNpcsParameters _npcParametersForQuestLine;
        [SerializeField] private QuestItemsParameters _itemParametersForQuestLine;
        [SerializeField] private List<EnemySO> _enemySos;
        [SerializeField] private List<NpcSO> _npcSos;
        [SerializeField] private List<ItemSO> _itemSos;

        public QuestDungeonsParameters DungeonParametersForQuestLine => _dungeonParametersForQuestLine;

        public QuestEnemiesParameters EnemyParametersForQuestLine => _enemyParametersForQuestLine;

        public QuestNpcsParameters NpcParametersForQuestLine => _npcParametersForQuestLine;

        public QuestItemsParameters ItemParametersForQuestLine => _itemParametersForQuestLine;

        public List<DungeonFileSO> DungeonFileSos => _dungeonFileSos;

        public List<EnemySO> EnemySos => _enemySos;

        public List<NpcSO> NpcSos => _npcSos;

        public List<ItemSO> ItemSos => _itemSos;
        
        public virtual void Init()
        {
            graph = new List<QuestSO>();
            _dungeonFileSos = new List<DungeonFileSO>();
            _enemySos = new List<EnemySO>();
            _npcSos = new List<NpcSO>();
            _itemSos = new List<ItemSO>();
        }

        public void CreateAsset(PlayerProfile.PlayerProfileCategory playerProfileCategory)
        {
            // Define the JSON file extension
            const string extension = ".asset";

            // Build the target path
            string target = Application.dataPath;
            target += Constants.SEPARATOR_CHARACTER + "Resources";
            target += Constants.SEPARATOR_CHARACTER + "Experiment";
            target += Constants.SEPARATOR_CHARACTER + playerProfileCategory.ToString();

            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            string fileName = "Narrative_" + graph[0] + extension;

            AssetDatabase.CreateAsset(this, fileName);

            /*            Resources.UnloadUnusedAssets();
            #if UNITY_EDITOR
                        AssetDatabase.Refresh();
            #endif*/
        }
    }
}
