using System;

namespace ServerReplayPlayer.Contracts
{
    [Serializable]
    public class MatchResultEntity : IWithId
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime Player1CreationDate { get; set; }
        public DateTime? Player2CreationDate { get; set; }
        public Guid Player { get; set; }
        public Guid Player2 { get; set; }
        public string Points { get; set; }
    }
}
