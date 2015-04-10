using CommonTypes;

namespace ServerReplayPlayer.Contracts
{
    public class CompetitionsViewModel
    {
        public CompetitionsInfo[] CompetitionsInfos { get; set; }
        public CommandEntity Command { get; set; }
        public string BackLevel { get; set; }
    }
}
