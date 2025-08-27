using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.EnemyGenerator;

public interface IEnemyFitness
{
    void Calculate(ref Individual _individual, float goal);
    bool IsBest(Individual _i1, Individual _i2);
}
