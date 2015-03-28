using System.IO;
using System.Web.Hosting;

namespace ServerReplayPlayer.Logic
{
    static class Helpers
    {
        public static void CreateDirectoryIfNoExists(this string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static string GetServerPath(string path)
        {
            return Path.Combine(HostingEnvironment.ApplicationPhysicalPath, path);
        }
    }
}
