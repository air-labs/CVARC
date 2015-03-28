using System;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    public interface IFileCache<TEntity> where TEntity : IWithId
    {
        void Save(TEntity entity, byte[] file);
        TEntity TryGetEntity(Func<TEntity, bool> selector);
        TEntity GetEntity(Guid id);
        TEntity[] GetAllEntities();
        byte[] GetFile(Guid id);
    }
}