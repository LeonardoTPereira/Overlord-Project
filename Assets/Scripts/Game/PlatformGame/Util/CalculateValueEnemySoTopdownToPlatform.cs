using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Util
{
    public class CalculateValueEnemySoTopdownToPlatform
    {
        // Since the values of attack speed is fixed for the top-down game (see in SearchSpace.cs)
        // for platform enemies is needed to recalculate its values using new max-min values
        public static float TopdownToPlatform(float enemySoValue, float minPlatValue, float maxPlatValue, float minTopDownValue, float maxTopDownValue)
        {
            return minPlatValue + (maxPlatValue - minPlatValue) * (enemySoValue - minTopDownValue) / (maxTopDownValue - minTopDownValue);
        }
    }
}
