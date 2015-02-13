namespace NetworkCompetitionsPlayer.Contracts
{
    public class CompetitionsInfoClient
    {
        public string[] Players { get; set; }
        public MatchResultClient[] MatchResults { get; set; }
    }
}
