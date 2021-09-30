using UnityEngine;

namespace ScriptableObjects
{
    public class NpcSO
    {
        public string NpcName
        {
            get => npcName;
            set => npcName = value;
        }

        [SerializeField] private Sprite npcSprite;

        private string npcName;
        /*TODO add NPC settings according to Akira and Yago's generator*/
        
    }
}
