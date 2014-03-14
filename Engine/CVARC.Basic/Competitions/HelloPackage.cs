using System.Runtime.Serialization;

namespace CVARC.Network
{
    public enum Side
    {
        Left,
        Right,
        Random
    }

    [DataContract]
    public class HelloPackage 
    {
        [DataMember]
        public string AccessKey { get; set; }
        
        [DataMember]
        public Side Side { get; set; }

        [DataMember]
        public string Opponent { get; set; }
        
        [DataMember]
        public int MapSeed { get; set; }
    }
}
