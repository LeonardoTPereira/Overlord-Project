namespace Util
{
    public static class Normalizer
    {
        public static float GetMinMaxNormalization(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }
    }
}