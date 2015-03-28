using CVARC.Basic;
using CVARC.Basic.Core.Participants;

namespace CVARC.Network
{
    class CompetitionsSettings
    {
        public Competitions Competitions { get; set; }
        public Participant[] Participants { get; set; }
        public bool RealTime { get; set; }
        public bool NeedSaveReplay { get; set; }
    }
}
