using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CVARC
{
    public class CommandLineData
    {
        public readonly List<string> Unnamed = new List<string>();
        public readonly Dictionary<string, string> Named = new Dictionary<string, string>();

        public static CommandLineData Parse(string[] args)
        {
            bool namedMode = false;
            string key = null;
            var data = new CommandLineData();
            foreach (var e in args)
            {
                if (e.StartsWith("-"))
                {
                    if (key != null)
                        data.Named[key] = "";
                    namedMode = true;
                    key = e.Substring(1);
                    continue;
                }
                if (key != null)
                {
                    data.Named[key] = e;
                    key = null;
                    continue;
                }
                if (namedMode)
                {
                    throw new Exception("You cannot specify default arguments after named ones");
                }
                data.Unnamed.Add(e);
            }
            if (key != null)
                data.Named[key] = "";
            return data;
        }
    }
}
