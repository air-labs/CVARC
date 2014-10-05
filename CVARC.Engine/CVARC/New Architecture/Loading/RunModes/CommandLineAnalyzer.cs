using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class CommandLineAnalyzer
    {
        public static RunModeArguments Analyze(string[] args)
        {
            var cmd = Parse(args);
            if (cmd.Unnamed.Count != 3)
            {
                throw new Exception("Exactly three unnamed arguments are expected: CVARC.exe Assembly Level Mode");
            }

            var arguments = new RunModeArguments();
            arguments.Assembly = cmd.Unnamed[0];
            arguments.Level = cmd.Unnamed[1];
            arguments.Mode = cmd.Unnamed[2];
            if (cmd.Named.ContainsKey("Seed"))
            {
                try
                {
                    arguments.Seed = int.Parse(cmd.Named["Seed"]);
                    cmd.Named.Remove("Seed");
                }
                catch
                {
                    throw new Exception("The -Seed argument must be integer");
                }
            }

            foreach (var e in cmd.Named)
                arguments.ControllersInfo.Add(e.Key,e.Value);
            return arguments;
        }

        static CommandLineData Parse(string[] args)
        {
            bool namedMode = false;
            string key = null;
            var data = new CommandLineData();
            foreach (var e in args)
            {
                if (e.StartsWith("-"))
                {
                    namedMode = true;
                    key = e.Substring(1);
                    continue;
                }
                if (key!=null)
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
            return data;
        }
    }
}

