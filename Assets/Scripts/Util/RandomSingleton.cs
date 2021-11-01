using System;

namespace Util
{
    public class RandomSingleton
    {
        private const int randomSeed = 42;
        private RandomSingleton()
        {
            random = new Random(randomSeed);
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