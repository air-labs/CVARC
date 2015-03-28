using System;

namespace ServerReplayPlayer.Contracts
{
    [Serializable]
    public class PlayerEntity : IWithId
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
    }
}