using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public abstract class EnemyMovementsSO<TEnum> : EnemyMovementsSOInterface where TEnum : Enum
    {
        [SerializeField] public List<TEnum> _enemyMovements;

        public override string GetMovementName(int index)
        {
            if (index < 0 || index >= Enum.GetValues(typeof(TEnum)).Length)
                return string.Empty;

            return ((TEnum)(object)index).ToString();
        }

        public override Enum GetEnemyMovementByIndex(int index)
        {
            if (index < 0 || index >= _enemyMovements.Count)
                throw new IndexOutOfRangeException($"Movement index {index} is out of range.");
            return _enemyMovements[index];
        }

        public override List<Enum> GetAllMovementTypes()
        {
            return ((TEnum[])Enum.GetValues(typeof(TEnum))).Cast<Enum>().ToList();
        }

        // RETIRAR APOS SEPARAR SCRIPTS QUE MEXEM COM INIMIGOS ESPECÍFICOS DO TOPDOWN
        public override List<Enum> GetHealerMovementList()
        {
            return new List<Enum>();
        }

        public override int GetEnemyMovementCount()
        {
            return _enemyMovements.Count;
        }
    }
}