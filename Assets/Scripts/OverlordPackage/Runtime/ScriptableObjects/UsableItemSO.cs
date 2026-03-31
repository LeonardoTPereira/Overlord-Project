using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "UsableItemSO", menuName = "Overlord-Project/Rules-Generator/Other/UsableItem")]
    public class UsableItemSO : ItemSo
    {
        /*TODO implement strategies*/
        public delegate bool ItemEffect();
    }
}
