using System;
using System.Text;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu] [Serializable]
    public class WeaponTypeSo : ScriptableObject
    {
        [field: SerializeField] public float FitnessMultiplier { get; set; }
        [field: SerializeField] public bool HasProjectile { get; set; }
        [field: SerializeField] public ProjectileTypeSO Projectile { get; set; }

        [field: SerializeField] public GameObject WeaponPrefab { get; set; }
        [field: SerializeField] public string EnemyTypeName { get; set; }

        [field: SerializeField] public bool HasSprite = true;

        [field: SerializeField] public bool IsPlatformGame = false;
                       

        public string RealTypeName()
        {
            if (IsPlatformGame)
            {
                switch(EnemyTypeName)
                {
                    case "Red Mage":
                        return "Shadow";
                    case "Green Mage":
                        return "Infected Ant";
                    case "Blue Mage":
                        return "Furious Ant";
                    case "Slime":
                        return "Gray Wolf";
                    default:
                        return "Black Wolf";
                }
            }
            return EnemyTypeName;
        }
        
        public bool IsHealer()
        {
            if (IsPlatformGame)
                return UnityEngine.Random.Range(0, 100) > 50f;

            return EnemyTypeName == "Healer";
        }

        public bool IsRanger()
        {
            return HasProjectile;
        }

        public bool IsMelee()
        {
            return !IsRanger() && !IsHealer();
        }
        
        public bool IsSword()
        {
            if (IsPlatformGame)
                return RealTypeName() == "Infected Ant" || RealTypeName() == "Furious Ant";

            return EnemyTypeName == "Sword";
        }

        public object GetEnemySpriteString()
        {
            if (!HasSprite)
                return "";

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"<sprite=\"Enemies\" name=\"{RealTypeName()}\">");
            Debug.Log(stringBuilder.ToString());
            return stringBuilder.ToString();
        }
    }
}