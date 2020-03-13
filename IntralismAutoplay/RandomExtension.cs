using System;

namespace IntralismAutoplay
{
    public static class RandomExtension
    {
        //For Humanizing stuff (If I remember correct!)
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}
