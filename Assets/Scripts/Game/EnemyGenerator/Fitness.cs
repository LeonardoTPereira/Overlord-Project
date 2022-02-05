using System;
using System.Diagnostics;

namespace Game.EnemyGenerator
{
    /// This class holds all the fitness-related functions.
    public static class Fitness
    {
        /// The error message of cannot compare individuals.
        public static readonly string CANNOT_COMPARE_INDIVIDUALS =
            "There is no way of comparing two null individuals.";

        /// Calculate the fitness value of the entered individual.
        ///
        /// An individual's fitness is defined by the distance of the
        /// individual's difficulty and the difficulty goal.
        public static void Calculate(
            ref Individual _individual,
            float goal
        ) {
            _individual.FitnessValue = Math.Abs(goal - _individual.DifficultyLevel);
        }

        /// Return true if the first individual (`_i1`) is best than the second
        /// (`_i2`), and false otherwise.
        ///
        /// The best is the individual that is closest to the goal in the
        /// MAP-Elites population. This is, the best is the one that's fitness
        /// has the lesser value. If `_i1` is null, then `_i2` is the best
        /// individual. If `_i2` is null, then `_i1` is the best individual. If
        /// both individuals are null, then the comparison cannot be performed.
        public static bool IsBest(
            Individual _i1,
            Individual _i2
        ) {
            Debug.Assert(
                _i1 != null || _i2 != null,
                CANNOT_COMPARE_INDIVIDUALS
            );
            if (_i1 is null) { return false; }
            if (_i2 is null) { return true; }
            return _i2.FitnessValue > _i1.FitnessValue;
        }
    }
}