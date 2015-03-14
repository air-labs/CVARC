using System;

namespace ServerReplayPlayer.Contracts
{
    public class ReplaysViewModel
    {
        public string Level { get; set; }
        public Summary[] Replays { get; set; }
    }

    public class Summary
    {
        public string Points { get; set; }
        public string Player { get; set; }
        public string Player2 { get; set; }
        public Guid Id { get; set; }

        public Summary(Guid id, string points, string player, string player2 = null)
        {
            Points = points;
            Player = player;
            Player2 = player2;
            Id = id;
        }
    }
}