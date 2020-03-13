using System;

namespace IntralismAutoplay
{
    class Program
    {
        //Calling the Program wininit..
        //Project is outdated, so getting values from ingame is not working anymore.
        //Need new Pointers (workshopID, current Time)
        static void Main()
        {
            Console.Title = "wininit";

            GameValueReader reader = new GameValueReader();

            string workshopID = reader.GetBeatmapID();
            int id = -1;
            int.TryParse(workshopID, out id);
            Console.WriteLine("[SEARCHING] Searching Workshop ID...");

            searchBeatmap:
            if (workshopID == "UnityEngine.Events.U")
            {
                workshopID = reader.GetBeatmapID();
                int.TryParse(workshopID, out id);
                goto searchBeatmap;
            }

            Console.WriteLine("[FOUND] Got workshop ID:" + workshopID + ", ID: " + id);
            Keyboard.PrepareHook();
            ProcessBeatMap(id);
            Console.ReadLine();
        }

        //When beatmap is finished, Program automatically shuts itself down,
        //else, it searches for a Beatmap, sets its Preset and Plays the beatmap.
        static void ProcessBeatMap(int? id) {
            if (id == null) return;

            BeatmapInfo beatmapInfo = new BeatmapInfo(id.Value);
            using (BeatmapPlayer beatmapPlayer = new BeatmapPlayer(beatmapInfo))
            {
                beatmapPlayer.BeatmapFinish += () => Environment.Exit(0);
                beatmapPlayer.SetPreset();
                beatmapPlayer.PlayBeatmap();
            }
        }
    }
}
