using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CVARC.V2
{
    [Serializable]
    public enum ControllerType
    {
        Bot,
        Client
    }

    [Serializable]
    [DataContract]
    public class ControllerConfiguration
    {
        [DataMember]
        public string ControllerId { get; set; }
        [DataMember]
        public ControllerType Type { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    [Serializable]
    public partial class Configuration
    {

        [DataMember] public string Assembly { get; set; }
        [DataMember] public string Level { get; set; }
        [DataMember] public RunModes Mode { get; set; }
        [DataMember] public int Seed { get; set; }
        [DataMember] public double? TimeLimit { get; set; }
        [DataMember] public bool EnableLog { get; set; }
        [DataMember] public string LogFile { get; set; }
        [DataMember] public bool SpeedUp { get; set; }
        [DataMember] public int Port { get; set; }
        [DataMember] public double OperationalTimeLimit { get; set; }
        
        [DataMember]
        public List<ControllerConfiguration> Controllers { get; private set; }

        public Configuration()
        {
            Controllers = new List<ControllerConfiguration>();
        }
    }


}
