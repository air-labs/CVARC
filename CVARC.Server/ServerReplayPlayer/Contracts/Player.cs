using System;

namespace ServerReplayPlayer.Contracts
{
    [Serializable]
    public class Player : IWithId
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}