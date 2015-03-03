using System;

namespace ServerReplayPlayer.Contracts
{
    [Serializable]
    public class MatchResultServer : IWithId
    {
        public MatchResultServer()
        {
        }

        public MatchResultServer(params string[] fields)
        {
            Player = fields[0];
            Player2 = fields[1];
        }

        public Guid Id { get; set; }
        public string Player { get; set; }
        public string Player2 { get; set; }
        public string Replay { get; set; }
    }
}