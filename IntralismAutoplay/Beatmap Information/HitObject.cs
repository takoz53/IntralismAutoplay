using System;
using System.Collections.Generic;

namespace IntralismAutoplay.Beatmap_Information
{
    [Serializable]
    class HitObject
    {
        public double Time { get; set; }
        public List<string> Data { get; set; }
    }
}
