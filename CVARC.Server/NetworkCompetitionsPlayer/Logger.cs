using System;
using System.IO;

namespace NetworkCompetitionsPlayer
{
    class Logger
    {
        public static void Log(Exception e)
        {
            File.AppendAllText("D:\\cvarcLog.txt", e.ToString());
        }
    }
}
