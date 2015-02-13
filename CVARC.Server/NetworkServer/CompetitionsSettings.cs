using CVARC.Basic;
using CVARC.Basic.Core.Participants;

namespace CVARC.Network
{
    public class CompetitionsSettings
    {
        public bool RealTime { get; set; }
        public CompetitionsBundle CompetitionsBundle { get; set; }
        public Participant[] Participants { get; set; }
    }
}
