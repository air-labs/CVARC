namespace ServerReplayPlayer.Contracts
{
    public class CompetitionsInfoServer
    {
        public string[] Players { get; set; }
        public MatchResultEntity[] MatchResults { get; set; }
    }
}
