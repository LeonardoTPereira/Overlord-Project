using System.Collections.Generic;
using System.IO;
using Game.GameManager;
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
        [SerializeField] private List<DungeonFileSo> _dungeonFileSos;
        [Serializeable] private QuestEnemiesParameters _enemyParametersForQuestLine;
        private QuestNpcsParameters _npcParametersForQuestLine;
        private QuestItemsParameters _itemParametersForQuestLine;
        [SerializeField] private List<EnemySO> _enemySos;
        [SerializeField] private List<NpcSO> _npcSos;
        [SerializeField] private List<ItemSO> _itemSos;
        private QuestDungeonsParameters _dungeonParametersForQuestLine;

        public QuestDungeonsParameters DungeonParametersForQuestLine
        {
            get => _dungeonParametersForQuestLine;
            set => _dungeonParametersForQuestLine = value;
        }

        public QuestEnemiesParameters EnemyParametersForQuestLine
        {
            get => _enemyParametersForQuestLine;
            set => _enemyParametersForQuestLine = value;
        }

        public QuestNpcsParameters NpcParametersForQuestLine => _npcParametersForQuestLine;

        public QuestItemsParameters ItemParametersForQuestLine => _itemParametersForQuestLine;

        public List<DungeonFileSo> DungeonFileSos => _dungeonFileSos;

        public List<EnemySO> EnemySos => _enemySos;

        public List<NpcSO> NpcSos => _npcSos;

        public List<ItemSO> ItemSos => _itemSos;
        
        public virtual void Init()
        {
            graph = new List<QuestSO>();
            _dungeonFileSos = new List<DungeonFileSo>();
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
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(this, fileName);

            /*            Resources.UnloadUnusedAssets();
            
                        AssetDatabase.Refresh();*/
#endif
        }

        public void CreateDummyEnemyParameters()
        {
            EnemyParametersForQuestLine = new QuestEnemiesParameters();
            var enemiesByType = new Dictionary<WeaponTypeSO, int>();
            int totalEnemies = 0;
            foreach (var weaponType in GameManagerSingleton.instance.enemyLoader.WeaponTypes.Items)
            {
                enemiesByType.Add(weaponType, 10);
                totalEnemies += 10;
            }
            EnemyParametersForQuestLine.TotalByType = enemiesByType;
            EnemyParametersForQuestLine.NEnemies = totalEnemies;
        }
    }
}
