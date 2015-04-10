using System;

namespace CommonTypes
{
    public class MatchResult
    {
        public Guid Id { get; set; }
        public Player Player { get; set; }
        public Player Player2 { get; set; }
        public int PlayerPoints { get; set; }
        public int Player2Points { get; set; }
        public bool IsMatchPlayed { get; set; }
        public string Replay { get; set; }
    }
}