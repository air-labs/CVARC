using System;

namespace CommonTypes
{
    public class MatchResult
    {
        public Guid Id { get; set; }
        public Player Player { get; set; }
        public Player Player2 { get; set; }
        public string Points { get; set; }
        public string Replay { get; set; }
        public DateTime Player1CreationDate { get; set; }
        public DateTime? Player2CreationDate { get; set; }
    }
}