using System.IO;

namespace ServerReplayPlayer.Logic
{
    static class Helpers
    {
        public static void CreateDirectoryIfNoExists(this string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
