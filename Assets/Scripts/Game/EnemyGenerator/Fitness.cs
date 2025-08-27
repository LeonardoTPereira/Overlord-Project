using Overlord.GenerationController.Facade;
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
        )
        {
            Fitness.CalculateFitnessFactor(ref _individual);
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
        )
        {
            Debug.Assert(
                _i1 != null || _i2 != null,
                CANNOT_COMPARE_INDIVIDUALS
            );
            if (_i1 is null) { return false; }
            if (_i2 is null) { return true; }
            return _i2.FitnessValue > _i1.FitnessValue;
        }    

    /// This class holds the enemy difficulty function.
    ///
    /// This difficulty function calculates four factors: health, strength,
    /// movement, and gameplay. Since one of the factors is the gameplay, this
    /// function depends on the in-game behavior of the enemies. This version
    /// handles only the gameplay of the game prototype mentioned in Program.cs.

        private const float HighBonus = 1.25f;
        private const float HighPenalty = 0.5f;
        private const float LowBonus = 1.15f;

        /// Calculate the difficulty of the entered individual.
        private static void CalculateFitnessFactor(
            ref Individual individual
        )
        {
            // Calculate all the difficulty factors
            float fH = CalculateHealthFactor(individual);
            float fS = CalculateStrengthFactor(individual);
            float fM = CalculateMovementFactor(individual);
            float fG = CalculateGameplayFactor(individual);
            // Calculate the final difficulty
            individual.DifficultyLevel = (fH + fS + fM) * fG;
        }

        /// Return the health factor.
        private static float CalculateHealthFactor(
            Individual _individual
        )
        {
            return _individual.Enemy.Status1 * 2;   // Status1 is the enemy health
        }

        /// Calculate and return the movement factor.
        private static float CalculateMovementFactor(
            Individual _individual
        )
        {
            // Create an alias for the enemy gene of the individual
            EnemyData e = _individual.Enemy;
            // Calculate movement factor
            float fM = e.Status4;
            // Both active time and rest time affect the behavior regarding
            // the enemies' movements, not the enemies' battles
            fM += e.Status5 / 3 + 1 / e.Status6;
            return fM;
        }

        /// Calculate and return the strength factor.
        private static float CalculateStrengthFactor(
            Individual _individual
        )
        {
            // Create aliases for the genes of the individual
            EnemyData e = _individual.Enemy;
            WeaponData w = _individual.Weapon;
            // Calculate strength factor
            float fS = 1;
            // Melee enemies attack by touching the player, therefore, the
            // movement speed increase their strenght
            fS *= SearchSpace.MeleeWeaponList().Contains(w.Weapon) ?
                e.Status2 * e.Status4 : 1;
            // Shooter enemies attack by throwing projectiles, then we count
            // both attack speed (shooting frequency) and projectile speed
            // Besides, the projectiles have the same damage
            fS *= SearchSpace.RangedWeaponList().Contains(w.Weapon) ?
                (e.Status3 * w.WeaponStatus1) * 3 : 1;
            // The cooldown of healer enemies follows the attack speed
            fS *= w.Weapon == WeaponType.CureSpell ?
                e.Status3 * 2 : 1;
            return fS;
        }

        /// Calculate and return the gameplay factor.
        ///
        /// The gameplay weights were empirically chosen based on the gameplay
        /// of the game prototype mentioned in Program.cs
        private static float CalculateGameplayFactor(Individual individual)
        {
            var enemy = individual.Enemy;
            var weapon = individual.Weapon;
            var gameplayFactor = 1f;
            if (SearchSpace.MeleeWeaponList().Contains(weapon.Weapon))
            {
                gameplayFactor = CalculateMeleeWeaponGameplayFactor(enemy, gameplayFactor);
            }
            else if (SearchSpace.RangedWeaponList().Contains(weapon.Weapon))
            {
                gameplayFactor = CalculateRangedWeaponGameplayFactor(enemy, gameplayFactor);
            }
            if (weapon.Weapon == WeaponType.CureSpell)
            {
                gameplayFactor = CalculateHealerGameplayFactor(enemy, gameplayFactor);
            }
            return gameplayFactor;
        }

        private static float CalculateHealerGameplayFactor(EnemyData e, float fG)
        {
            if (!RulesGeneratorFacade.Instance.GetEnemyMovementType().GetHealerMovementList().Contains(e.Movement))
                fG *= HighPenalty;

            fG *= e.Status4 * 1.15f;
            return fG;
        }

        private static float CalculateRangedWeaponGameplayFactor(EnemyData e, float fG)
        {
            switch (e.Movement)
            {
                case TopdownEnemyMovementsSO.MovementTypeEnums.Flee1D:
                    fG *= LowBonus;
                    break;
                case TopdownEnemyMovementsSO.MovementTypeEnums.Flee:
                    fG *= HighBonus;
                    break;
                case TopdownEnemyMovementsSO.MovementTypeEnums.None:
                    fG *= HighPenalty;
                    break;
                case TopdownEnemyMovementsSO.MovementTypeEnums.Follow:
                    fG *= HighPenalty / (e.Status4 * 2);
                    break;
            }

            return fG;
        }

        private static float CalculateMeleeWeaponGameplayFactor(EnemyData e, float fG)
        {
            switch (e.Movement)
            {
                case TopdownEnemyMovementsSO.MovementTypeEnums.Follow:
                    fG *= HighBonus;
                    break;
                case TopdownEnemyMovementsSO.MovementTypeEnums.None:
                case TopdownEnemyMovementsSO.MovementTypeEnums.Flee1D:
                case TopdownEnemyMovementsSO.MovementTypeEnums.Flee:
                    fG *= HighPenalty;
                    break;
            }

            return fG;
        }
    }
}