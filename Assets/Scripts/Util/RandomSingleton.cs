using System;
using UnityEngine;
using Random = System.Random;

namespace Util
{
    public class RandomSingleton
    {
        private RandomSingleton()
        {
            var seed = (int) DateTime.Now.Ticks & 0x0000FFFF;
            random = new Random(seed);
            #if UNITY_EDITOR
                Debug.Log("Random Seed: " + seed);
            #endif
        }

        private static RandomSingleton _instance;
        protected Random random;

        public Random Random { get => random; set => random = value; }

        public static RandomSingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RandomSingleton();
            }
            return _instance;
        }
    }
}