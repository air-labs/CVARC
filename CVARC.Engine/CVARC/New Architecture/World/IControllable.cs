using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic.Core.Participants;

namespace CVARC.V2
{
    public interface IControllable
    {
        string ControllerId { get; }
        void AcceptParticipant(IController participant);
    }
}
