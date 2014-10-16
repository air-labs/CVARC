using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CVARC.V2
{
 

    [DataContract]
    [Serializable]
    public partial class Configuration
    {
        [DataMember]
        public LoadingData LoadingData { get; set; }
        [DataMember]
        public Settings Settings { get; set; }
        
    }


}
