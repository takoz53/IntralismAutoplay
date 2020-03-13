using System;

namespace IntralismAutoplay
{
    class PresetSelector
    {
        public double StartMinStart { get; }
        public double StartMinEnd { get; }
        public double StartMaxStart { get; }
        public double StartMaxEnd { get; }
        public double RandomMin { get; }
        public double RandomMax { get; }
        public PresetSelector(double startMin, double startMax, double randomMin, double randomMax, int valStart, double valOffset)
        {
            var numberToDivideWith = Math.Pow(10, valStart);
            StartMinStart = startMin;
            StartMinEnd = startMin - (valOffset / numberToDivideWith);
            StartMaxStart = startMax;
            StartMaxEnd = startMax + (valOffset / numberToDivideWith);
            RandomMin = randomMin;
            RandomMax = randomMax;
        }
    }
}
