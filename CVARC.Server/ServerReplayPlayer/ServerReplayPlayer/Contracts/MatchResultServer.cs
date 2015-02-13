namespace ServerReplayPlayer.Contracts
{
    public class MatchResultServer
    {
        public MatchResultServer(params string[] fields)
        {
            Player = fields[0];
            Player2 = fields[1];
        }

        public string Player { get; set; }
        public string Player2 { get; set; }
        public string Points { get; set; }
        public byte[] Replay { get; set; }
    }
}