using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CVARC.V2
{
    

    [DataContract]
    public class TournamentConfiguration<TWorldState>
    {
        [DataMember]
        public List<ControllerSettings> ControllerSettings { get; set; }

        [DataMember]
        public TWorldState WorldState { get; set; }
    }
}
