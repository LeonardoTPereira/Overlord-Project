using UnityEngine;
using UnityEngine.Serialization;

namespace  ScriptableObjects
{
    [CreateAssetMenu]
    public class NpcDialogueSO : ScriptableObject
    {
        public NpcSO Npc
        {
            get => npc;
            set => npc = value;
        }

        [SerializeField, TextArea] private string[] dialogues;
        [FormerlySerializedAs("_npc")] [SerializeField] private NpcSO npc;

        public string[] Dialogues
        {
            get => dialogues;
            set => dialogues = value;
        }
    }
}
