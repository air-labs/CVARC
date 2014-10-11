using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic.Core.Participants;

namespace CVARC.V2
{
    public interface IControllable : IActor
    {
        string ControllerId { get; }
        void ExecuteCommand(ICommand command);
        object GetSensorData();
    }
}
