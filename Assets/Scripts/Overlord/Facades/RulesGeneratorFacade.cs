using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlord.GenerationController.Facade
{
    public class RulesGeneratorFacade<IEnemy>
    {
        public List<IEnemy> GetEnemies()
        {
            List<IEnemy> enemies = new List<IEnemy>();
            return enemies;
        }
        
    }

    public interface IEnemy
    {
        
    }
}

