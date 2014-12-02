using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using System.Runtime.Serialization;

namespace Demo
{
	[DataContract]
    public class MovementWorldState : IWorldState
    {
		[DataMember]
		public bool RectangularRobot { get; set; }
        [DataMember]
        public bool objects { get; set; }
    }
}
