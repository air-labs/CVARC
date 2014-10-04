using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IActor
    {
        Type GetManagerType { get; }
        Type GetWorldType { get; }
        string ObjectId { get; }
        IWorld World { get; }
        void Tick(double time);
        void Initialize(IActorManager manager, IWorld world, string objectId);
    }
}
