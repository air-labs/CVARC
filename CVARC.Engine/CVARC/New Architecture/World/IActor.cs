using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic.Core.Participants;

namespace CVARC.V2
{

    public interface IActor
    {
        Type GetManagerType { get; }
        Type GetWorldType { get; }
        string ObjectId { get; }
        IWorld World { get; }
        void Initialize(IActorManager manager, IWorld world, string objectId);
        string ControllerId { get; }
        void ExecuteCommand(ICommand command);
        object GetSensorData();
    }
}
