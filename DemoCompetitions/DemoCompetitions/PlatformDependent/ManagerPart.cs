﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace DemoCompetitions
{
    public class DemoManagerPart : ManagerPart 
    {
        public DemoManagerPart() : base(new DemoWorldManager(),
                new IActorManagerFactory[] { new ActorManagerFactory<DemoActorManager>() })
        {}
    }
}