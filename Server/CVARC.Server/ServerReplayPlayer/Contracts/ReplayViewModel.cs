using System;

namespace ServerReplayPlayer.Contracts
{
    public class ReplayViewModel
    {
        public string Level { get; set; }
        public Guid Id { get; set; }
        public string RedPoints { get; set; }
        public string BluePoints { get; set; }

        public ReplayViewModel(string level, Guid id, string redPoints, string bluePoints)
        {
            Level = level;
            Id = id;
            RedPoints = redPoints;
            BluePoints = bluePoints;
        }
    }
}