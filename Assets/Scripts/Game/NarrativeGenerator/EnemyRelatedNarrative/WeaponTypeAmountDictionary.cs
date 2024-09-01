using System;
using ScriptableObjects;

namespace Game.NarrativeGenerator.EnemyRelatedNarrative
{
    [Serializable]
    public class WeaponTypeAmountDictionary : KeyByQuestIdsDictionary<WeaponTypeSo>
    {
        public WeaponTypeAmountDictionary()
        {
        }

        public WeaponTypeAmountDictionary(KeyByQuestIdsDictionary<WeaponTypeSo> keyByQuestIdsDictionary) : base(keyByQuestIdsDictionary)
        {
        }

        public new object Clone()
        {
            return new WeaponTypeAmountDictionary(this);
        }
    }
}