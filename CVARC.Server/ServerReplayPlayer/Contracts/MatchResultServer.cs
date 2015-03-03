using System;

namespace ServerReplayPlayer.Contracts
{
    [Serializable]
    public class MatchResultServer
    {
        public MatchResultServer()
        {
        }

        public MatchResultServer(params string[] fields)
        {
            Player = fields[0];
            Player2 = fields[1];
        }

        public string Player { get; set; }
        public string Player2 { get; set; }
        public string Replay { get; set; }
        public bool IsFinished { get; set; }
    }
}