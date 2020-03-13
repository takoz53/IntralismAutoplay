using System;
using System.IO;

namespace IntralismAutoplay
{
    class MapSelector
    {
        //If u have custom Path, check here.
        private const string BEATMAP_PATH = @"C:\Program Files (x86)\Steam\steamapps\workshop\content\513510";
        //Gets Beatmap from ID and tries to read it.
        public string GetBeatmap(int beatmapID)
        {
            if (!File.Exists(Path.Combine(BEATMAP_PATH, Convert.ToString(beatmapID), "config.txt")))
                return String.Empty;
            string beatmapLocation = Path.Combine(BEATMAP_PATH, Convert.ToString(beatmapID), "config.txt");
            var responseText = String.Empty;
            using (StreamReader reader = new StreamReader(beatmapLocation))
            {
                
                while (reader.Peek() >= 0)
                {
                    responseText += (char) reader.Read();
                }
            }
            if(responseText == String.Empty)
                throw new Exception("Couldn't read Beatmap with ID" + beatmapID + "\nLocation: " + beatmapLocation);
            return responseText;
        }
    }
}
