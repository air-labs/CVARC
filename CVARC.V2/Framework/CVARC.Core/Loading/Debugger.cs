using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public enum DebuggerMessageType
    {
        Unity,
        Protocol,
        Error,
        Workflow
    }
    public static class Debugger
    {
        public static bool DisableByDefault { get; set; }
        public static List<DebuggerMessageType> EnabledTypes { get; private set; }

        static Debugger()
        {
            EnabledTypes = new List<DebuggerMessageType>();
            File.Delete("log.txt");
        }
        public static void Log(DebuggerMessageType messageType, string str)
        {
            if (DisableByDefault && !EnabledTypes.Contains(messageType))
                return;
            if (Logger != null)
                Logger(str);
            File.AppendAllText("log.txt", str + "\n");
        }
        public static Action<string> Logger;
    }
}
