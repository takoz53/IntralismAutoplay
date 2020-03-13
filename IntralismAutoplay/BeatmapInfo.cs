using System;
using System.Collections.Generic;
using IntralismAutoplay.Beatmap_Information;
using Newtonsoft.Json;

namespace IntralismAutoplay
{
    class BeatmapInfo
    {
        private readonly int beatmapID;
        private readonly MapSelector mapSelector;
        public BeatmapInfo(int beatmapID)
        {
            this.beatmapID = beatmapID;
            mapSelector = new MapSelector();
        }

        //Basic information about a Beatmap
        private Beatmap GetBeatmapInformation()
        {
            var beatmapJSON = mapSelector.GetBeatmap(beatmapID);
            if (beatmapJSON == String.Empty)
                return null;
            var beatmapInformation = JsonConvert.DeserializeObject<Beatmap>(beatmapJSON);
            return beatmapInformation;
        }

        //Adds HitObjects into a List to process them later.
        public List<HitObject> GetBeatmapHitObjects()
        {
            Beatmap beatmap = GetBeatmapInformation();
            if (beatmap == null)
                return null;
            List<HitObject> hitObjects = new List<HitObject>();
            foreach (HitObject hitObject in beatmap.HitObjects)
            {
                List<string> dataList = new List<string>();
                bool validData = false;
                foreach (var data in hitObject.Data)
                {
                    if (data == "SpawnObj")
                    {
                        dataList.Add(data);
                        validData = true;
                        continue;
                    }
                    else if (data.Contains("Up") || data.Contains("Left") || data.Contains("Down") || data.Contains("Right"))
                    {
                        dataList.Add(data);
                        validData = true;
                    }
                }

                if (validData)
                    hitObjects.Add(new HitObject() {Data = dataList, Time = hitObject.Time});
            }

            return hitObjects;
        }
    }
}
