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
        UnityTest,
        Protocol,
        Error,
        Workflow,
        Drawing,
        Initialization,
        Always
    }
    public static class Debugger
    {
        public static bool DisableByDefault { get; set; }
        public static List<object> EnabledTypes { get; private set; }

        static object lockObject = new object();

        static Debugger()
        {
            EnabledTypes = new List<object>();
            File.Delete("log.txt");
        }
        public static void Log(object messageType, object obj)
        {
            lock (lockObject)
            {
                var str = obj.ToString();
                if (DisableByDefault && !EnabledTypes.Contains(messageType) && !messageType.Equals(DebuggerMessageType.Always))
                    return;
                if (Logger != null)
                    Logger(str);
                File.AppendAllText("log.txt", str + "\n");
            }
        }

        public static void Mark()
        {
            Log(DebuggerMessageType.Always, "!");
        }

        public static Action<string> Logger;
    }
}
