using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IntralismAutoplay.Beatmap_Information
{
    //Beatmap information taken from a Beatmap File, broken down into a JSON Structure
    [Serializable]
    class Beatmap
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public ICollection<LevelResource> LevelResources { get; set; }
        public ICollection<string> Tags { get; set; }
        public int HandCount { get; set; }
        public string MoreInfoURL { get; set; }
        public double Speed { get; set; }
        public int Lives { get; set; }
        public int MaxLives { get; set; }
        public string MusicFile { get; set; }
        public double MusicTime { get; set; }
        public string IconFile { get; set; }
        public int GenerationType { get; set; }
        public int EnvironmentType { get; set; }
        public ICollection<string> UnlockConditions { get; set; }
        public bool Hidden { get; set; }
        public List<double> Checkpoints { get; set; }
        public ICollection<string> PuzzleSequencesList { get; set; }
        [JsonProperty("Events")]
        public List<HitObject> HitObjects { get; set; }
    }
}
