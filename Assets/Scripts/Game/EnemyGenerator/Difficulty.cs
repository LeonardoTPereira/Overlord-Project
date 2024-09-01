namespace Game.EnemyGenerator
{
    /// This class holds the enemy difficulty function.
    ///
    /// This difficulty function calculates four factors: health, strength,
    /// movement, and gameplay. Since one of the factors is the gameplay, this
    /// function depends on the in-game behavior of the enemies. This version
    /// handles only the gameplay of the game prototype mentioned in Program.cs.
    public static class Difficulty
    {
        private const float HighBonus = 1.25f;
        private const float HighPenalty = 0.5f;
        private const float LowBonus = 1.15f;
        
        /// Calculate the difficulty of the entered individual.
        public static void Calculate(
            ref Individual individual
        ) {
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
        ) {
            return _individual.Enemy.Health * 2;
        }

        /// Calculate and return the movement factor.
        private static float CalculateMovementFactor(
            Individual _individual
        ) {
            // Create an alias for the enemy gene of the individual
            EnemyData e = _individual.Enemy;
            // Calculate movement factor
            float fM = e.MovementSpeed;
            // Both active time and rest time affect the behavior regarding
            // the enemies' movements, not the enemies' battles
            fM += e.ActiveTime / 3 + 1 / e.RestTime;
            return fM;
        }

        /// Calculate and return the strength factor.
        private static float CalculateStrengthFactor(
            Individual _individual
        ) {
            // Create aliases for the genes of the individual
            EnemyData e = _individual.Enemy;
            WeaponData w = _individual.Weapon;
            // Calculate strength factor
            float fS = 1;
            // Melee enemies attack by touching the player, therefore, the
            // movement speed increase their strenght
            fS *= SearchSpace.MeleeWeaponList().Contains(w.Weapon) ?
                e.Strength * e.MovementSpeed : 1;
            // Shooter enemies attack by throwing projectiles, then we count
            // both attack speed (shooting frequency) and projectile speed
            // Besides, the projectiles have the same damage
            fS *= SearchSpace.RangedWeaponList().Contains(w.Weapon) ?
                (e.AttackSpeed * w.ProjectileSpeed) * 3 : 1;
            // The cooldown of healer enemies follows the attack speed
            fS *= w.Weapon == WeaponType.CureSpell ?
                e.AttackSpeed * 2 : 1;
            return fS;
        }

        /// Calculate and return the gameplay factor.
        ///
        /// The gameplay weights were empirically chosen based on the gameplay
        /// of the game prototype mentioned in Program.cs
        private static float CalculateGameplayFactor(Individual individual) {
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
            if (!SearchSpace.HealerMovementList().Contains(e.Movement))
            {
                fG *= HighPenalty;
            }

            fG *= e.MovementSpeed * 1.15f;
            return fG;
        }

        private static float CalculateRangedWeaponGameplayFactor(EnemyData e, float fG)
        {
            switch (e.Movement)
            {
                case MovementType.Flee1D:
                    fG *= LowBonus;
                    break;
                case MovementType.Flee:
                    fG *= HighBonus;
                    break;
                case MovementType.None:
                    fG *= HighPenalty;
                    break;
                case MovementType.Follow:
                    fG *= HighPenalty / (e.MovementSpeed * 2);
                    break;
            }

            return fG;
        }

        private static float CalculateMeleeWeaponGameplayFactor(EnemyData e, float fG)
        {
            switch (e.Movement)
            {
                case MovementType.Follow:
                    fG *= HighBonus;
                    break;
                case MovementType.None:
                case MovementType.Flee1D:
                case MovementType.Flee:
                    fG *= HighPenalty;
                    break;
            }

            return fG;
        }
    }
}