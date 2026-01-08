using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public abstract class EnemyMovementsSO<TEnum> : EnemyMovementsSOInterface where TEnum : Enum
    {
        [SerializeField] public List<TEnum> _enemyMovements;
        private Dictionary<Enum, int> _movementIndexMap;

        public override int GetMappedIndex(Enum movement)
        {
            if (_movementIndexMap == null)
            {
                _movementIndexMap = new Dictionary<Enum, int>();
                for (int i = 0; i < _enemyMovements.Count; i++)
                {
                    _movementIndexMap[_enemyMovements[i]] = i;
                }
            }

            if (_movementIndexMap.TryGetValue(movement, out int index))
            {
                return index;
            }

            throw new ArgumentException($"Movimento '{movement}' não encontrado na lista de movimentos do inimigo.");
        }

        public override string GetMovementName(int index)
        {
            if (index < 0 || index >= _enemyMovements.Count)
                return string.Empty;

            return _enemyMovements[index].ToString();
        }

        public override Enum GetEnemyMovementByIndex(int index)
        {
            if (index < 0 || index >= _enemyMovements.Count)
                throw new IndexOutOfRangeException($"Movement index {index} is out of range.");
            return _enemyMovements[index];
        }

        public override List<Enum> GetAllMovementTypes()
        {
            //return ((TEnum[])Enum.GetValues(typeof(TEnum))).Cast<Enum>().ToList();
            return _enemyMovements.Cast<Enum>().ToList();
        }

        public override List<Enum> GetAllMovementEnums()
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