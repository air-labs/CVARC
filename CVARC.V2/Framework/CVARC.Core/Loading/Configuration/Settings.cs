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
    public class ControllerSettings
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
    public class Settings
    {
        [DataMember] 
        public int Seed { get; set; }
        
        [DataMember] 
        public double TimeLimit { get; set; }
       
        [DataMember] 
        public bool EnableLog { get; set; }
        
        [DataMember] 
        public string LogFile { get; set; }
        
        [DataMember] 
        public bool SpeedUp { get; set; }
        
        [DataMember] 
        public double OperationalTimeLimit { get; set; }
        
        [DataMember]
        public int Port { get; set; }

        [DataMember]
        public string SolutionsFolder { get; set; }
        
        [DataMember]
        public List<ControllerSettings> Controllers { get; private set; }

        public Settings()
        {
            Controllers = new List<ControllerSettings>();
            Port = 14000;
        }
    }
}
