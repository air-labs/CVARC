using System;

namespace ServerReplayPlayer.Contracts
{
    public class ReplayViewModel
    {
        public string Level { get; set; }
        public Guid Id { get; set; }

        public ReplayViewModel(string level, Guid id)
        {
            Level = level;
            Id = id;
        }
    }
}