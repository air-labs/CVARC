namespace ServerReplayPlayer.Contracts
{
    public class CompetitionsInfoServer
    {
        public string[] Players { get; set; }
        public MatchResultServer[] MatchResults { get; set; }
    }
}
