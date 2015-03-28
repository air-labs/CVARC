using System;

namespace ServerReplayPlayer.Contracts
{
    public interface IWithId
    {
        Guid Id { get; set; }
        DateTime CreationDate { get; set; }
    }
}