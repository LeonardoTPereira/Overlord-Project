namespace Game.EnemyGenerator
{
    /// This class holds the enemy difficulty function.
    ///
    /// This difficulty function calculates four factors: health, strength,
    /// movement, and gameplay. Since one of the factors is the gameplay, this
    /// function depends on the in-game behavior of the enemies. This version
    /// handles only the gameplay of the game prototype mentioned in Program.cs.
    public class Difficulty
    {
        /// Calculate the difficulty of the entered individual.
        public static void Calculate(
            ref Individual _individual
        ) {
            // Calculate all the difficulty factors
            float fH = Difficulty.CalculateHealthFactor(_individual);
            float fS = Difficulty.CalculateStrengthFactor(_individual);
            float fM = Difficulty.CalculateMovementFactor(_individual);
            float fG = Difficulty.CalculateGameplayFactor(_individual);
            // Calculate the final difficulty
            _individual.difficulty = (fH + fS + fM) * fG;
        }

        /// Return the health factor.
        private static float CalculateHealthFactor(
            Individual _individual
        ) {
            return _individual.enemy.health * 2;
        }

        /// Calculate and return the movement factor.
        private static float CalculateMovementFactor(
            Individual _individual
        ) {
            // Create an alias for the enemy gene of the individual
            Enemy e = _individual.enemy;
            // Calculate movement factor
            float fM = e.movementSpeed;
            // Both active time and rest time affect the behavior regarding
            // the enemies' movements, not the enemies' battles
            fM += e.activeTime / 3 + 1 / e.restTime;
            return fM;
        }

        /// Calculate and return the strength factor.
        private static float CalculateStrengthFactor(
            Individual _individual
        ) {
            // Create aliases for the genes of the individual
            Enemy e = _individual.enemy;
            Weapon w = _individual.weapon;
            // Calculate strength factor
            float fS = 1;
            // Melee enemies attack by touching the player, therefore, the
            // movement speed increase their strenght
            fS *= SearchSpace.MeleeWeaponList().Contains(w.weaponType) ?
                e.strength * e.movementSpeed : 1;
            // Shooter enemies attack by throwing projectiles, then we count
            // both attack speed (shooting frequency) and projectile speed
            // Besides, the projectiles have the same damage
            fS *= SearchSpace.RangedWeaponList().Contains(w.weaponType) ?
                (e.attackSpeed * w.projectileSpeed) * 3 : 1;
            // The cooldown of healer enemies follows the attack speed
            fS *= w.weaponType == WeaponType.CureSpell ?
                e.attackSpeed * 2 : 1;
            return fS;
        }

        /// Calculate and return the gameplay factor.
        ///
        /// The gameplay weights were empirically chosen based on the gameplay
        /// of the game prototype mentioned in Program.cs
        public static float CalculateGameplayFactor(
            Individual _individual
        ) {
            // Create aliases for the genes of the individual
            Enemy e = _individual.enemy;
            Weapon w = _individual.weapon;
            // Calculate the gameplay factor
            float fG = 1f;
            // Melee enemies are only risky if they follow the player
            fG *= SearchSpace.MeleeWeaponList().Contains(w.weaponType) ?
                (e.movementType == MovementType.Follow ? 1.25f : 1) : 1;
            fG *= SearchSpace.MeleeWeaponList().Contains(w.weaponType) ?
                (e.movementType == MovementType.None ||
                 e.movementType == MovementType.Flee1D ||
                 e.movementType == MovementType.Flee ? 0.5f : 1) : 1;
            // Shooter enemies that flee are riskier to the player
            fG *= SearchSpace.RangedWeaponList().Contains(w.weaponType) ?
                (e.movementType == MovementType.Flee1D ? 1.15f : 1) : 1;
            fG *= SearchSpace.RangedWeaponList().Contains(w.weaponType) ?
                (e.movementType == MovementType.Flee ? 1.25f : 1) : 1;
            // Shooter enemies that stay still are the only ones that present
            // some risk to the player since they throw projectiles towards them
            fG *= SearchSpace.RangedWeaponList().Contains(w.weaponType) ?
                (e.movementType == MovementType.None ? 0.5f : 1) : 1;
            // Shooter enemies do not perform well when they follow the player
            // and are faster than the projectiles they shoot
            fG *= (SearchSpace.RangedWeaponList().Contains(w.weaponType) &&
                e.movementType == MovementType.Follow) ?
                0.5f / (e.movementSpeed * 2) : 1;
            // Healer enemies must avoid the player and search for other enemies
            fG *= (w.weaponType == WeaponType.CureSpell ?
                (SearchSpace.HealerMovementList().Contains(e.movementType)
                ? 1 : 0.5f) : 1);
            // Healer enemies must move fast to avoid the player
            fG *= w.weaponType == WeaponType.CureSpell ?
                e.movementSpeed * 1.15f : 1;
            return fG;
        }
    }
}