using System;
using System.Text;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "Items/Item"), Serializable]
    public class ItemSo : ScriptableObject
    {
        [SerializeField]
        public Sprite sprite;
        [field: SerializeField] public String ItemName { get; set; }
        [field: SerializeField] public int Value { get; set; }

        public string GetGemstoneSpriteString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"<sprite=\"Gemstones\" name=\"{ItemName}\">");
            return stringBuilder.ToString();
        }
        
        public string GetToolSpriteString()
        {
            var stringBuilder = new StringBuilder();    
            stringBuilder.Append($"<sprite=\"Tools\" name=\"{ItemName}\">");
            return stringBuilder.ToString();
        }
    }
}
