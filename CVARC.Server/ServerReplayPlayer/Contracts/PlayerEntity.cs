using System;

namespace ServerReplayPlayer.Contracts
{
    [Serializable]
    public class PlayerEntity : IWithId
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}