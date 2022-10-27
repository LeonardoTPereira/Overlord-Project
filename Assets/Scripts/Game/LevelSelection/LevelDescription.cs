using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.LevelSelection
{
    public class LevelDescription : MonoBehaviour
    {
        public string DungeonDescription { get; set; }
        public string QuestDescription { get; set; }

        [field:SerializeField] public TextMeshProUGUI DisplayedText { get; set; }

        protected bool _isShowingDungeon;

        public virtual void CreateDescriptions(LevelData levelData)
        {
            CreateDungeonDescription(levelData.Dungeon);
            _isShowingDungeon = false;
            ChangeDescription();
        }

        private void CreateDungeonDescription(DungeonFileSo dungeon)
        {
            DungeonDescription = "";
            DungeonDescription += "Rooms: " + dungeon.Parts.Count;
            DungeonDescription += "\nFitness: " + dungeon.FitnessFromEa.NormalizedResult;
            DungeonDescription += "\nExploration: " + dungeon.ExplorationCoefficient;
            DungeonDescription += "\nLeniency: " + dungeon.LeniencyCoefficient;
        }

        public void ChangeDescription(InputAction.CallbackContext context)
        {
            ChangeDescription();
        }

        protected void ChangeDescription()
        {
            _isShowingDungeon = !_isShowingDungeon;
            DisplayedText.text = _isShowingDungeon ? DungeonDescription : QuestDescription;
        }
    }
}