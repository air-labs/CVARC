using System;
using System.IO;

namespace NetworkCompetitionsPlayer
{
    class Logger
    {
        public static void Log(Exception e)
        {
            File.AppendAllText("C:\\cvarcLog.txt", e.ToString());
        }
    }
}
