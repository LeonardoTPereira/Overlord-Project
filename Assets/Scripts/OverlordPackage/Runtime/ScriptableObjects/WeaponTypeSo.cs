using System;
using System.Text;
using UnityEngine;
using static Util.Enums;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponTypeSo", menuName = "Overlord-Project/Rules-Generator/Util/WeaponType")]
    [Serializable]
    public class WeaponTypeSo : ScriptableObject
    {
        [field: SerializeField] public float FitnessMultiplier { get; set; }
        [field: SerializeField] public bool HasProjectile { get; set; }
        [field: SerializeField] public ProjectileTypeSO Projectile { get; set; }

        [field: SerializeField] public GameObject WeaponPrefab { get; set; }
        [field: SerializeField] public string EnemyTypeName { get; set; }
        [field: SerializeField] public WeaponTypeEnum Type { get; set; }


        [field: SerializeField] public bool HasSprite = true;
        [field: SerializeField] public bool IsPlatformGame = false;

        public string RealTypeName(bool isInPortuguese)
        {
            if (IsPlatformGame)
            {
                if (isInPortuguese)
                {
                    switch (EnemyTypeName)
                    {
                        case "Red Mage":
                            return "Sombra";
                        case "Green Mage":
                            return "Formiga Infectada";
                        case "Blue Mage":
                            return "Formiga Furiosa";
                        case "Slime":
                            return "Lobo Cinzento";
                        default:
                            return "Lobo Preto";
                    }
                }
                else
                {
                    switch (EnemyTypeName)
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
            }
            return EnemyTypeName;
        }

        public string RealTypeName(Language language)
        {
            switch (language)
            {                 
                case Language.Portuguese:
                    return RealTypeName(true);
                case Language.English:
                    return RealTypeName(false);
                default:
                    return RealTypeName(false);
            }
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

        public bool IsSword(bool isInPortuguese)
        {
            if (IsPlatformGame)
            {
                if (isInPortuguese)
                    return (RealTypeName(isInPortuguese) == "Formiga Infectada" || RealTypeName(isInPortuguese) == "Formiga Furiosa");
                return (RealTypeName(isInPortuguese) == "Infected Ant" || RealTypeName(isInPortuguese) == "Furious Ant");
            }

            return EnemyTypeName == "Sword";
        }

        public object GetEnemySpriteString(bool isInPortuguese)
        {
            if (!HasSprite)
                return "";

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"<sprite=\"Enemies\" name=\"{RealTypeName(isInPortuguese)}\">");
            Debug.Log(stringBuilder.ToString());
            return stringBuilder.ToString();
        }

        public object GetEnemySpriteString(Language language)
        {
            switch (language)
            {
                case Language.Portuguese:
                    return GetEnemySpriteString(true);
                case Language.English:
                    return GetEnemySpriteString(false);
                default:
                    return GetEnemySpriteString(false);
            }
        }
    }
}