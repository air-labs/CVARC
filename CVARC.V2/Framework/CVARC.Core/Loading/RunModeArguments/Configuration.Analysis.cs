using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    partial class Configuration
    {
        static T GetArgument<T>(CommandLineData data, string key, Func<string, T> parser, string exceptionMessage, T defaultValue)
        {
            if (data.Named.ContainsKey(key))
            {
                try
                {
                    return parser(data.Named[key]);
                }
                catch
                {
                    throw new Exception(exceptionMessage);
                }
            }
            return defaultValue;
        }


        public static Configuration Analyze(string[] args)
        {
            var cmd = Parse(args);
            var arguments = new Configuration(); 
            
            if (cmd.Unnamed.Count == 3)
            {
                arguments.Assembly = cmd.Unnamed[0];
                arguments.Level = cmd.Unnamed[1];
                try
                {
                    arguments.Mode = (RunModes)Enum.Parse(typeof(RunModes), cmd.Unnamed[2]);
                }
                catch
                {
                    var modes = Enum.GetNames(typeof(RunModes)).Aggregate((a, b) => a + ", " + b);
                    throw new Exception("The mode " + cmd.Unnamed[2] + " is unknown. Try one of these: " + modes);
                }
                arguments.LogFile = GetArgument<string>(cmd, "LogFile", s => s, "", null);
            }
            else if (cmd.Unnamed.Count == 1)
            {
                arguments.LogFile = cmd.Unnamed[0];
                arguments.Mode = RunModes.Play;
            }
            else
            {
                throw new Exception("The program requires 3 unnamed arguments for a normal work, or one argument for playing logs");
            }




            arguments.Seed = GetArgument<int>(cmd, "Seed", int.Parse, "The -Seed argument must be integer", 0);
            arguments.TimeLimit = GetArgument<double?>(cmd, "TimeLimit", s => double.Parse(s), "The -TimeLimit argument must be floating point", null);
            
            arguments.EnableLog = GetArgument<bool>(cmd, "EnableLog", s => true, "", false);
            arguments.SpeedUp = GetArgument<bool>(cmd, "SpeedUp", s => true, "", false);
            arguments.OperationalTimeLimit = GetArgument<double>(cmd, "OpTL", s => double.Parse(s), "Operational time limit must be floating point number", double.PositiveInfinity);

            var ControllerPrefix = "Controller.";
            foreach (var e in cmd.Named.Where(z=>z.Key.StartsWith(ControllerPrefix)))
                arguments.Controllers.Add(e.Key.Substring(ControllerPrefix.Length),e.Value);
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
                    if (key != null)
                        data.Named[key] = "";
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
            if (key != null)
                data.Named[key] = "";
            return data;
        }
    }
}

