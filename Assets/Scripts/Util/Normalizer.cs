namespace Util
{
    public static class Normalizer
    {
        public static float GetMinMaxNormalization(float value, float min, float max)
        {
            if (max - min == 0)
            {
                return value;
            }
            return (value - min) / (max - min);
        }
    }
}