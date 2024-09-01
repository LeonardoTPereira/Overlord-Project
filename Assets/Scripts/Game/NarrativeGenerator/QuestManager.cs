using UnityEngine;

namespace Game.NarrativeGenerator
{
    [CreateAssetMenu(fileName = "QuestManager", menuName = "ScriptableObjects/QuestManager")]
    public class QuestManager : ScriptableObject
    {
        public int totalEnemies = 0, totalItens = 0, totalNpcs = 0;
        public bool secretRoom = false, treasure = false;
    }
}