using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NpcSO", menuName = "Overlord-Project/Npcs", order = 0)]    
    public class NpcSO : ScriptableObject
    {
        public string NpcName
        {
            get => npcName;
            set => npcName = value;
        }

        [SerializeField] private Sprite npcSprite;
        [FormerlySerializedAs("_npcName")] [SerializeField] private string npcName;

        /*TODO add NPC settings according to Akira and Yago's generator*/
        
    }
}
