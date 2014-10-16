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
    public class SettingsProposal
    {
        [DataMember]
        public int? Seed { get; set; }
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
        public List<ControllerSettings> Controllers { get; set; }



        public void Parse<T>(
            CommandLineData data,
            Expression<Func<SettingsProposal, T>> propertyLambda, 
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
            property.SetValue(this, arg, new object[0]);
        }


        public static SettingsProposal FromCommandLineData(CommandLineData data)
        {
            var proposal = new SettingsProposal();
            proposal.Parse(data, z => z.EnableLog, s => true, "");
            proposal.Parse(data, z => z.LogFile, s => s, "");
            proposal.Parse(data, z => z.OperationalTimeLimit, s => double.Parse(s, CultureInfo.InvariantCulture), "OperationalTimeLimit must be floating point number");
            proposal.Parse(data, z => z.Seed, s => int.Parse(s), "Seed must be integer number");
            proposal.Parse(data, z => z.SpeedUp, s => true, "");
            proposal.Parse(data, z => z.TimeLimit, s => double.Parse(s, CultureInfo.InvariantCulture), "TimeLimit must be floating point number");

            var ControllerPrefix = "Controller.";

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


        public void Push(Settings settings, bool pushAllFields, params Expression<Func<Settings,object>>[] fieldsToPush)
        {
            PropertyInfo[] props;
            if (!pushAllFields)
                props = typeof(Settings).GetProperties();
            else
                props = fieldsToPush
                    .Select(z => z.Body)
                    .Cast<MemberExpression>()
                    .Select(z => z.Member)
                    .Cast<PropertyInfo>()
                    .ToArray();
            foreach (var p in props)
            {
                var p1 = typeof(SettingsProposal).GetProperty(p.Name);
                if (p1 == null) throw new Exception("Property " + p.Name + " is defined in Settings but not in SettingsProposal");
                var value = p1.GetValue(this, new object[0]);
                var flag = (bool)value.GetType().GetProperty("HasValue").GetValue(value, new object[0]);
                if (!flag) continue;
                value = value.GetType().GetProperty("Value").GetValue(value, new object[0]);
                p.SetValue(settings, value, new object[0]);
            }
        }
    }
}
