using System;
using System.Collections.Generic;
using System.Linq;
using IntralismAutoplay.Beatmap_Information;
using Timer = System.Timers.Timer;

namespace IntralismAutoplay
{
    public delegate void OnBeatmapFinishedHandler();

    class BeatmapPlayer : IDisposable
    {
        private readonly BeatmapInfo beatmapInfo;
        private readonly GameValueReader reader;
        private Timer timer;
        private Dictionary<double, KeyDirection> playObj;
        private int currentNote;
        public event OnBeatmapFinishedHandler BeatmapFinish;
        private PresetSelector presetSelector;

        public BeatmapPlayer(BeatmapInfo beatmapInfo)
        {
            playObj = new Dictionary<double, KeyDirection>();
            currentNote = 0;
            this.beatmapInfo = beatmapInfo;
            reader = new GameValueReader();

        }

        //Preset calculations are wrong, need tweaking a lot.
        public void SetPreset()
        {
            int preset = -1;
            while (preset <= 0 || preset > 3)
            {
                Console.Write("Please set a preset\n1: 96% Accuracy\n2: 98% Accuracy\n3: 99% Accuracy [1-3]:");
                string presetSelected = Console.ReadLine();
                int.TryParse(presetSelected, out preset);
            }

            switch (preset)
            {
                //96%~ Preset
                case 1:
                    presetSelector = new PresetSelector(-0.024, 0.024, -0.009, 0.009, 2, 0.95);
                    break;
                //98%~ Preset
                case 2:
                    presetSelector = new PresetSelector(-0.015, 0.015, -0.008, 0.008, 2, 1);
                    break;
                //99%~ Preset
                case 3:
                    presetSelector = new PresetSelector(-0.005, 0.005, -0.005, 0.005, 4, 5);
                    break;
                case 4:
                    presetSelector = new PresetSelector(-0.02, 0.02, -0.009, 0.09, 2, 1);
                    break;
                default:
                    break;
            }
            Console.WriteLine("[USER] Selected preset: " + preset);
            WindowManager.SetFocusToExternalApp("Intralism");
        }
        //This function needs to be reworked generally as I have wrong calculations
        //The function should humanize clicks more with random offsets.
        private List<HitObject> HumanizeClicks()
        {
            var hitObjects = beatmapInfo.GetBeatmapHitObjects();
            if (hitObjects == null)
                return null;
            var modifiedHitObjects = new List<HitObject>();

            Random rnd = new Random();
            var startOffsetMin = rnd.NextDouble(presetSelector.StartMinStart, presetSelector.StartMinEnd);
            var startOffsetMax = rnd.NextDouble(presetSelector.StartMaxStart, presetSelector.StartMaxEnd);

            var startOffset = 0.0;
            var OffsetToPick = rnd.Next(2);
            Console.WriteLine("[DEBUG] The picked Offset is " + OffsetToPick);
            Console.WriteLine("[DEBUG] 0 Early | 1 Late");
            if (OffsetToPick == 0)
                startOffset = startOffsetMin;
            else
                startOffset = startOffsetMax;

            foreach (var hitObject in hitObjects)
            {
                var randomOffset = rnd.NextDouble(startOffset + presetSelector.RandomMin, startOffset + presetSelector.RandomMax);
                var newTime = Math.Round(hitObject.Time + randomOffset, 8);
                modifiedHitObjects.Add(new HitObject() {Data = hitObject.Data, Time = newTime});
            }

            return modifiedHitObjects;
        }

        //Creates a Dictionary with Key Directions to simulate clicks.
        private Dictionary<double, KeyDirection> GetClicks()
        {
            var clickInformation = new Dictionary<double, KeyDirection>();
            var hitObjects = HumanizeClicks();
            if (hitObjects == null)
                return null;

            foreach (var hitObject in hitObjects)
            {
                KeyDirection value = 0;
                foreach (var data in hitObject.Data)
                {
                    if (data == "SpawnObj")
                        continue;
                    if (data.Contains("Up"))
                        value |= KeyDirection.Up;
                    if (data.Contains("Down"))
                        value |= KeyDirection.Down;
                    if (data.Contains("Left"))
                        value |= KeyDirection.Left;
                    if (data.Contains("Right"))
                        value |= KeyDirection.Right;
                }

                clickInformation.Add(hitObject.Time, value);
            }

            return clickInformation;
        }

        //Main Function to play a beatmap.
        public void PlayBeatmap()
        {
            playObj = GetClicks();
            if (playObj == null)
                return;
            // I shouldn't solve this with a timer but watever
            timer = new Timer
            {
                Interval = 2
            };
            
            timer.Elapsed += CheckAndClick;

            Console.WriteLine("[READY] Bot is ready for the input.");
            timer.Start();
        }

        //Fakes keypresses
        //This should be humanized more: Key releases should differ
        private void PressKeys(double timeToPress)
        {
            KeyDirection keysToPress = playObj.ElementAt(currentNote).Value;
            Click(keysToPress, timeToPress);
        }

        //Clicking is delayed, so we have a constant number which clicks a little earlier to avoid late clicks
        //This function checks if there is a Note to click on.
        private void CheckAndClick(object sender, System.Timers.ElapsedEventArgs e)
        {
            const double CLICK_DELAY = .0016;
            if (playObj.Count - 1 < currentNote)
            {
                OnBeatmapFinish();
                return;
            }
            var curTime = reader.GetTime();
            var arcTime = playObj.ElementAt(currentNote);

            if (curTime >= arcTime.Key - CLICK_DELAY)
            {
                PressKeys(curTime);
                currentNote++;
            }
        }

        //Calls the real Keypress
        private void Click(KeyDirection direction, double time)
        {
            if ((direction & KeyDirection.Left) == KeyDirection.Left) {
                Keyboard.PressKey(VirtualKeyShort.KEY_D);
            }

            if ((direction & KeyDirection.Up) == KeyDirection.Up) {
                Keyboard.PressKey(VirtualKeyShort.KEY_F);
            }

            if ((direction & KeyDirection.Down) == KeyDirection.Down) {
                Keyboard.PressKey(VirtualKeyShort.KEY_J);
            }

            if ((direction & KeyDirection.Right) == KeyDirection.Right) {
                Keyboard.PressKey(VirtualKeyShort.KEY_K);
            }

            Console.WriteLine($"[EMULATING] {direction} at {time}s");
        }

        public void Dispose()
        {
            if (playObj == null) return;
            if (playObj.Count != currentNote) return;

            timer.Stop();
            timer = null;
            playObj = null;
            Environment.Exit(0);
        }

        [Flags]
        enum KeyDirection : byte
        {
            Right = 1 << 0,
            Down = 1 << 1,
            Up = 1 << 2,
            Left = 1 << 3,
        }

        public virtual void OnBeatmapFinish()
        {
            Dispose();
            BeatmapFinish?.Invoke();
        }
    }
}
