using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu, Serializable]
    public class ItemSo : ScriptableObject
    {
        [SerializeField]
        public Sprite sprite;
        [SerializeField]
        public int value;
    }
}
