using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CVARC.V2
{
    [DataContract]
	public class MoveAndBuildCommand : ICommand, ISimpleMovementCommand, ITowerBuilderCommand
	{
        [DataMember]
		public SimpleMovement SimpleMovement
		{
			get;
			set;
		}

        [DataMember]
        public TowerBuilderAction TowerBuilderCommand
        {
            get;
            set;
        }
    }
}
