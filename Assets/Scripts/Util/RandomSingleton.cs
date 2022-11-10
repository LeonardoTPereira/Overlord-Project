using System;
using System.Collections.Generic;
using Random = System.Random;

namespace Util
{
    public class RandomSingleton
    {
        private RandomSingleton()
        {
            var seed = (int) DateTime.Now.Ticks & 0x0000FFFF;
            Random = new Random(seed);
        }

        private static RandomSingleton _instance;

        public Random Random { get; set; }

        public static RandomSingleton GetInstance()
        {
            return _instance ??= new RandomSingleton();
        }

        public float Next(float min, float max)
        {
            return (float) Random.NextDouble() * (max - min) + min;
        }

        public int Next(int min, int max)
        {
            return Random.Next(min, max);
        }

        public int RandomPercent() {
            return Random.Next(100);
        }

        /// Return a random element from the entered array.
        public T RandomElementFromArray<T>(T[] range) {
            return range[Random.Next(0, range.Length)];
        }

        /// Return a random element from the entered list.
        public T RandomElementFromList<T>(List<T> range) {
            return range[Random.Next(0, range.Count)];
        }
    }
}