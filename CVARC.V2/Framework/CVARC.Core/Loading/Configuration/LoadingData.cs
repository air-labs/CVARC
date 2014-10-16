using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CVARC.V2
{
    [Serializable]
    [DataContract]
    public class LoadingData
    {
        [DataMember]
        public string AssemblyName { get; private set; }
        [DataMember]
        public string Level { get; private set; }
    }
}
