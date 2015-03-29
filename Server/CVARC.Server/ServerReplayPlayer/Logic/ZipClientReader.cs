using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;

namespace ServerReplayPlayer.Logic
{
    public class ZipClientReader
    {
        public const int MaxFileSize = 3 * 1024 * 1024;

        public static byte[] Read(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0 || file.ContentLength > MaxFileSize)
                return null;
            try
            {
                return ReadInternal(file);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        private static byte[] ReadInternal(HttpPostedFileBase file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                string clientName = null;
                var runableFileNames = new List<string>();
                using (ZipInputStream zipStream = new ZipInputStream(memoryStream))
                {
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
                                return null;
                            clientName = commands[2];
                            if (commands[0] != "start" || commands[1] != @"""rtsClient""" || !clientName.EndsWith(".exe") || commands[3] != "%*")
                                return null;
                        }
                    }
                }
                
                if (clientName == null || !runableFileNames.Contains(clientName))
                    return null;
                return memoryStream.ToArray();
            }
        }
    }
}
