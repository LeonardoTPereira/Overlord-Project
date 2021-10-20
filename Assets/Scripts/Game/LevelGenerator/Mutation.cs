using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace LevelGenerator
{
    /// This class holds the mutation operator.
    public class Mutation
    {
        /// The rate at which the mutation adds or removes a lock and a key.
        private static int MUTATION_TYPE_RATE = 50;

        /// Reproduce a new individual by mutating a parent.
        public static Individual Apply(
            Individual _parent,
            ref Random _rand
        ) {
            Individual individual = _parent.Clone();
            // Remove or add a lock and a key
            if (MUTATION_TYPE_RATE > Common.RandomPercent(ref _rand))
            {
                individual.dungeon.AddLockAndKey(ref _rand);
            }
            else
            {
                individual.dungeon.RemoveLockAndKey(ref _rand);
            }
            // Transfer enemies between rooms
            int size = individual.dungeon.rooms.Count;
            List<int> options = Enumerable.Range(1, size - 1).ToList<int>();
            while (options.Count / 2 > 1)
            {
                // Select random rooms
                int idx_f = Common.RandomElementFromList(options, ref _rand);
                options.Remove(idx_f);
                int idx_t = Common.RandomElementFromList(options, ref _rand);
                options.Remove(idx_t);
                // If the chosen room `to` has more enemies than the
                // room `from`, then swap them
                Room from = individual.dungeon.rooms[idx_f];
                Room to = individual.dungeon.rooms[idx_t];
                if (to.enemies > from.enemies)
                {
                    Room aux = from;
                    from = to;
                    to = aux;
                }
                // Transfer enemies
                int transfer = 0;
                if (from.enemies > 0)
                {
                    transfer = Common.RandomInt((1, from.enemies), ref _rand);
                }
                from.enemies -= transfer;
                to.enemies += transfer;
                Debug.Assert(from.enemies >= 0 && to.enemies >= 0);
            }
            return individual;
        }
    }
}