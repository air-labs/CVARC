using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CVARC.V2
{



    public class CommandLineConfigurationPull : IConfigurationPull
    {
        CommandLineData data;
        public CommandLineConfigurationPull(CommandLineData data)
        {
            this.data = data;
        }

        class Getter
        {
            public string[] fieldsToPull;
            public Settings cfg;
            public CommandLineData data;

            public void Make<T>(Expression<Func<Settings, T>> propertyLambda, Func<string, T> parser, string exceptionMessage)
            {
                var property = (propertyLambda.Body as MemberExpression).Member as PropertyInfo;
                var propertyName = property.Name;
                if (fieldsToPull != null && !fieldsToPull.Contains(propertyName)) return;
                if (!data.Named.ContainsKey(propertyName)) return;
                T arg;
                try
                {
                    arg = parser(data.Named[propertyName]);
                }
                catch
                {
                    throw new Exception(exceptionMessage);
                }
                property.SetValue(cfg, arg, new object[0]);
            }

        }

        public void Pull(Settings configuration, string[] onlyFields)
        {
            var puller = new Getter { cfg = configuration, fieldsToPull = onlyFields, data = data };
            puller.Make(z => z.EnableLog, s => true, "");
            puller.Make(z => z.LogFile, s => s, "");
            puller.Make(z => z.OperationalTimeLimit, s => double.Parse(s, CultureInfo.InvariantCulture), "OperationalTimeLimit must be floating point number");
            puller.Make(z => z.Port, s => int.Parse(s), "Port must be integer number");
            puller.Make(z => z.Seed, s => int.Parse(s), "Seed must be integer number");
            puller.Make(z => z.SpeedUp, s => true, "");
            puller.Make(z => z.TimeLimit, s => double.Parse(s, CultureInfo.InvariantCulture), "TimeLimit must be floating point number");

            if (!onlyFields.Contains("Controllers")) return;
            configuration.Controllers.Clear();
            var ControllerPrefix = "Controller.";
            foreach (var e in data.Named.Keys.Where(z => z.StartsWith(ControllerPrefix)))
            {
                var record = new ControllerSettings();
                record.ControllerId = e.Substring(ControllerPrefix.Length);
                var parts = data.Named[e].Split('.');
                if (parts.Length != 2)
                    throw new Exception("When specifying a controller, two parts must be defined, like Bot.Azura or Client.Ivan");
                try
                {
                    record.Type = (ControllerType)Enum.Parse(typeof(ControllerType), parts[0]);
                }
                catch
                {
                    throw new Exception("Unknown controller type '" + parts[0] + "', Bot or Client is expected");
                }
                record.Name = parts[1];
                configuration.Controllers.Add(record);
            }

        
        }
    }
}
