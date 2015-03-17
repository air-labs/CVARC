using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;

namespace ServerReplayPlayer.Logic
{
    public class FileValidator
    {
        public static bool IsValid(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return false;
            var fileName = Guid.NewGuid().ToString();
            string path = null;
            try
            {
                path = Storage.SaveTempFile(file, fileName);
                ZipFile.ExtractToDirectory(Path.Combine(path, fileName), path);
                var runBatPath = Path.Combine(path, "run.bat");
                if (!File.Exists(runBatPath))
                    return false;
                var line = File.ReadAllLines(runBatPath).FirstOrDefault();
                if (line == null)
                    return false;
                var commands = line.Split(new[] {" ", "    "}, StringSplitOptions.RemoveEmptyEntries);
                if (commands.Length < 4)
                    return false;
                var clientName = commands[2];
                if (commands[0] != "start" || commands[1] != @"""rtsClient""" || !clientName.EndsWith(".exe") ||
                    commands[3] != "%*")
                    return false;
                return File.Exists(Path.Combine(path, clientName));
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                try
                {
                    if (path != null)
                        Directory.Delete(path, true);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
