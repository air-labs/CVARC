using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{

    public interface IActor
    {
        string ObjectId { get; }
        IWorld World { get; }
        void Initialize(IActorManager manager, IWorld world, string objectId, string controllerId);
        string ControllerId { get; }
        void ExecuteCommand(ICommand command);
        object GetSensorData();
    }
}
