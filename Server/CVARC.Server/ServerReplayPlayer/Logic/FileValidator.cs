using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;

namespace ServerReplayPlayer.Logic
{
    public class FileValidator
    {
        public const int MaxFileSize = 3 * 1024 * 1024;

        public static bool IsValid(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0 || file.ContentLength > MaxFileSize)
                return false;
            try
            {
                return IsValidInternal(file);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }

        private static bool IsValidInternal(HttpPostedFileBase file)
        {
            using (ZipInputStream zipStream = new ZipInputStream(file.InputStream))
            {
                var runableFileNames = new List<string>();
                string clientName = null;
                ZipEntry temp;
                while ((temp = zipStream.GetNextEntry()) != null)
                {
                    if (temp.Name.EndsWith(".exe"))
                        runableFileNames.Add(temp.Name);
                    if (temp.Name == "run.bat")
                    {
                        var buffer = new byte[temp.Size];
                        zipStream.Read(buffer, (int)temp.Offset, (int)temp.Size);
                        var line = Encoding.UTF8.GetString(buffer);
                        var commands = line.Split(new[] { " ", "    ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (commands.Length < 4)
                            return false;
                        clientName = commands[2];
                        if (commands[0] != "start" || commands[1] != @"""rtsClient""" || !clientName.EndsWith(".exe") || commands[3] != "%*")
                            return false;
                    }
                }
                return clientName != null && runableFileNames.Contains(clientName);
            }
        }
    }
}
