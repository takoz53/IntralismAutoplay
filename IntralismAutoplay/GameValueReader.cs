using Memory;


namespace IntralismAutoplay
{
    class GameValueReader
    {
        private Mem memoryReader;

        public GameValueReader()
        {
            memoryReader = new Mem();
        }

        //Gets Map ID
        public string GetBeatmapID()
        {
            int processID = memoryReader.getProcIDFromName("Intralism.exe");
            memoryReader.OpenProcess(processID);
            //This needs a new Pointer. Getting this seems to be a lot harder now, good luck.
            return memoryReader.readString("mono.dll+0x0026AC30,A0,420,30,90,20,14", "", 40).Replace("workshop.", "");
        }
        //Gets Time of current Song
        public float GetTime()
        {
            int processID = memoryReader.getProcIDFromName("Intralism.exe");
            memoryReader.OpenProcess(processID);
            //Always need to open process before that, no idea why..
            //Also probably needs a new Timer.
            return memoryReader.readFloat("mono.dll+002659C8,138,5F8");
        }
    }
}
