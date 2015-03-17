using System;

namespace ServerReplayPlayer.Contracts
{
    [Serializable]
    public class MatchResultEntity : IWithId
    {
        public Guid Id { get; set; }
        public Guid Player { get; set; }
        public Guid Player2 { get; set; }
        public string Points { get; set; }
    }
}
