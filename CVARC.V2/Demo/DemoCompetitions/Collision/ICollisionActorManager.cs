using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace Demo
{
    public interface ICollisionActorManager : IActorManager
    {
        void Grip(string objectId);
    }
}
