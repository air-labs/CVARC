using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace CVARC.V2
{
    [DataContract]
    [Serializable]
    public class SettingsProposal : SettingsOrSettingsProposal
    {
        [DataMember]
        public double? TimeLimit { get; set; }
        [DataMember]
        public bool? EnableLog { get; set; }
        [DataMember]
        public string LogFile { get; set; }
        [DataMember]
        public bool? SpeedUp { get;  set; }
        [DataMember]
        public double? OperationalTimeLimit { get; set; }
        [DataMember]
        public int? Port { get; set; }
        [DataMember]
        public string SolutionsFolder { get; set; }
        [DataMember]
        public string LegacyLogFile { get; set; }
        [DataMember]
        public string WorldState { get; set; }
        [DataMember]
        public List<ControllerSettings> Controllers { get; set; }

        class Parser
        {
           public CommandLineData data;
           public SettingsProposal proposal;
           public List<string> unusedKeys;

            public void Parse<T>(Expression<Func<SettingsProposal, T>> propertyLambda,
            Func<string, T> parser,
            string exceptionMessage)
            {
                var property = (propertyLambda.Body as MemberExpression).Member as PropertyInfo;
                var propertyName = property.Name;
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
                property.SetValue(proposal, arg, new object[0]);
                unusedKeys.Remove(propertyName);
            }
        }


        public static SettingsProposal FromCommandLineData(CommandLineData data)
        {
             var ControllerPrefix = "Controller.";

            var proposal = new SettingsProposal();
            var parser = new Parser
            {
                data = data,
                proposal = proposal,
                unusedKeys = data.Named.Keys.Where(z => !z.StartsWith(ControllerPrefix)).ToList()
            };
            parser.Parse(z => z.EnableLog, s => s!="false", "");
            parser.Parse(z => z.LogFile, s => s, "");
            parser.Parse(z => z.OperationalTimeLimit, s => double.Parse(s, CultureInfo.InvariantCulture), "OperationalTimeLimit must be floating point number");
            parser.Parse(z => z.SpeedUp, s => s!="false", "");
            parser.Parse(z => z.TimeLimit, s => double.Parse(s, CultureInfo.InvariantCulture), "TimeLimit must be floating point number");
            parser.Parse(z => z.LegacyLogFile, s => s, "");
            parser.Parse(z => z.WorldState, s => s, "");

            if (parser.unusedKeys.Any())
            {
                throw new Exception("The key '"+parser.unusedKeys[0]+"' is unknown");
                }

            if (!data.Named.Keys.Any(s => s.StartsWith(ControllerPrefix))) return proposal;
            
            proposal.Controllers = new List<ControllerSettings>();
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
                proposal.Controllers.Add(record);
            }
            return proposal;
        }


        public void Push<T>(T settings, bool pushAllFields, params Expression<Func<T,object>>[] fieldsToPush)
            where T : SettingsOrSettingsProposal
        {
            PropertyInfo[] props;
            if (pushAllFields)
                props = typeof(T).GetProperties();
            else
                props = fieldsToPush
                    .Select(z => z.Body)
                    .Cast<UnaryExpression>()
                    .Select(z=>z.Operand)
                    .Cast<MemberExpression>()
                    .Select(z => z.Member)
                    .Cast<PropertyInfo>()
                    .ToArray();
            foreach (var p in props)
            {
                var p1 = typeof(SettingsProposal).GetProperty(p.Name);
                if (p1 == null)
                    continue;
                    //throw new Exception("Property " + p.Name + " is defined in Settings but not in SettingsProposal");
                var value = p1.GetValue(this, new object[0]);
                if (value == null) continue;
                p.SetValue(settings, value, new object[0]);
            }
        }
    }
}
