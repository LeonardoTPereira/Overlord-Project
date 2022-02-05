using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu, Serializable]
    public class ItemSo : ScriptableObject
    {
        [SerializeField]
        public Sprite sprite;
        [field: SerializeField] public int Value { get; set; }
    }
}
