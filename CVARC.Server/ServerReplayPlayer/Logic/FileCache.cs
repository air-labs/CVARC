using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    public class FileCache<TEntity> where TEntity : IWithId
    {
        private readonly string folder;
        private readonly ConcurrentDictionary<Guid, byte[]> cacheFile = new ConcurrentDictionary<Guid, byte[]>();
        private ConcurrentDictionary<Guid, TEntity> cacheEntity;
        private ConcurrentDictionary<Guid, TEntity> CacheEntity
        {
            get
            {
                return cacheEntity ?? (cacheEntity = new ConcurrentDictionary<Guid, TEntity>(Directory
                    .GetFiles(folder)
                    .Where(x => x.EndsWith(".entity"))
                    .Select(x =>
                    {
                        var id = Guid.Parse(Path.GetFileName(x));
                        return new KeyValuePair<Guid, TEntity>(id, ReadEntity(id));
                    })));
            }
        }

        public FileCache(string folder)
        {
            this.folder = folder;
            CreateDirectoryIfNotExsists(folder);
        }

        public void Save(TEntity entity, byte[] file)
        {
            var path = GetPath(entity.Id);
            using (var fileStream = File.Open(Path.Combine(path, ".entity"), FileMode.OpenOrCreate))
                new BinaryFormatter().Serialize(fileStream, entity);
            using (var writer = new BinaryWriter(File.Open(Path.Combine(path, ".file"), FileMode.OpenOrCreate)))
                writer.Write(file);
            CacheEntity.AddOrUpdate(entity.Id, x => entity, (x, y) => entity);
            cacheFile.AddOrUpdate(entity.Id, x => file, (x, y) => file);
        }

        public TEntity TryGetEntity(Func<TEntity, bool> selector)
        {
            return CacheEntity.Values.SingleOrDefault(selector);
        }

        public TEntity GetEntity(Guid id)
        {
            return CacheEntity.GetOrAdd(id, ReadEntity);
        }

        public TEntity[] GetAllEntities()
        {
            return CacheEntity.Select(x => x.Value).ToArray();
        }

        private TEntity ReadEntity(Guid id)
        {
            using (var file = File.Open(GetPath(id), FileMode.Open))
                return (TEntity)new BinaryFormatter().Deserialize(file);
        }

        public byte[] GetFile(Guid id)
        {
            return cacheFile.GetOrAdd(id, x => File.ReadAllBytes(Path.Combine(GetPath(id), ".file")));
        }

        private string GetPath(Guid id)
        {
            return Path.Combine(folder, id.ToString());
        }

        private void CreateDirectoryIfNotExsists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
