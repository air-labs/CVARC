using System;

namespace ServerReplayPlayer.Contracts
{
    [Serializable]
    public class MatchResult : IWithId
    {
        public Guid Id { get; set; }
        public string Player { get; set; }
        public string Player2 { get; set; }
        public string Points { get; set; }
    }
}
