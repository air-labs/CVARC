namespace NetworkCompetitionsPlayer.Contracts
{
    public class MatchResultClient
    {
        public string Player { get; set; }
        public string Player2 { get; set; }
        public byte[] Replay { get; set; }

        public bool IsFinished()
        {
            return false;
        }
    }
}