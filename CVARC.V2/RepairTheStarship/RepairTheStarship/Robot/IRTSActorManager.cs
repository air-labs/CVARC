using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace RepairTheStarship
{
    public interface IRTSActorManager : IActorManager
    {
        void Capture(string detailId);
        void Release(string detailId);
        bool IsDetailFree(string detailId);
    }
}
