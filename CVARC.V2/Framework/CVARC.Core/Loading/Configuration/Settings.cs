using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AIRLab.Mathematics;

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
    public class Settings : SettingsOrSettingsProposal
    {
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

        [DataMember]
        public Frame3D ObserverCameraLocation { get; set; }

        [DataMember]
        public string WorldState { get; set; }

        [DataMember]
        public string LegacyLogFile { get; set; }

        public Settings()
        {
            ObserverCameraLocation = new Frame3D(0, 0, 150, -Angle.HalfPi, Angle.Zero, -Angle.HalfPi);
            Controllers = new List<ControllerSettings>();
            Port = 14000;
            SolutionsFolder = "Solutions";
            OperationalTimeLimit = 1;
        }
    }
}
