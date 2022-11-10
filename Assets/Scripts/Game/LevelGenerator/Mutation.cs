using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Util;

namespace Game.LevelGenerator
{
    /// This class holds the mutation operator.
    public static class Mutation
    {
        /// The rate at which the mutation adds or removes a lock and a key.
        private static int MUTATION_TYPE_RATE = 50;

        /// Reproduce a new individual by mutating a parent.
        public static Individual Apply(Individual _parent) {
            Individual individual = new Individual(_parent);
            // Remove or add a lock and a key
            if (MUTATION_TYPE_RATE > RandomSingleton.GetInstance().RandomPercent())
            {
                individual.dungeon.AddLockAndKey();
            }
            else
            {
                individual.dungeon.RemoveLockAndKey();
            }
            // Transfer enemies between rooms
            var size = individual.dungeon.Rooms.Count;
            var options = Enumerable.Range(1, size-1).ToList();
            while (options.Count / 2 > 1)
            {
                // Select random rooms
                int idx_f = RandomSingleton.GetInstance().RandomElementFromList(options);
                options.Remove(idx_f);
                int idx_t = RandomSingleton.GetInstance().RandomElementFromList(options);
                options.Remove(idx_t);
                // If the chosen room `to` has more enemies than the
                // room `from`, then swap them
                Room from = individual.dungeon.Rooms[idx_f];
                Room to = individual.dungeon.Rooms[idx_t];
                if (to.Enemies > from.Enemies)
                {
                    Room aux = from;
                    from = to;
                    to = aux;
                }
                // Transfer enemies
                int transfer = 0;
                if (from.Enemies > 0)
                {
                    transfer = RandomSingleton.GetInstance().Next(1, from.Enemies+1);
                }
                from.Enemies -= transfer;
                to.Enemies += transfer;
                Debug.Assert(from.Enemies >= 0 && to.Enemies >= 0);
            }
            return individual;
        }
    }
}