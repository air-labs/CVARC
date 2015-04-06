using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    public class FileCache<TEntity> : IFileCache<TEntity> where TEntity : IWithId
    {
        private readonly string folder;
        private readonly bool needFileZip;
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
                        var id = Guid.Parse(Path.GetFileNameWithoutExtension(x));
                        return new KeyValuePair<Guid, TEntity>(id, ReadEntity(id));
                    })));
            }
        }

        public FileCache(string folder, bool needFileZip)
        {
            this.folder = folder;
            this.needFileZip = needFileZip;
            folder.CreateDirectoryIfNoExists();
        }

        public void Save(TEntity entity, byte[] file)
        {
            var path = GetPath(entity.Id);
            entity.CreationDate = DateTime.UtcNow;
            using (var fileStream = File.Open(path + ".entity", FileMode.Create))
                new BinaryFormatter().Serialize(fileStream, entity);
            using (var fileStream = GetFileStream(path, FileMode.Create, CompressionMode.Compress))
                fileStream.Write(file, 0, file.Length);
            CacheEntity.AddOrUpdate(entity.Id, x => entity, (x, y) => entity);
        }

        private Stream GetFileStream(string path, FileMode fileMode, CompressionMode compressionMode)
        {
            Stream stream = File.Open(path + ".file", fileMode);
            return needFileZip ? new GZipStream(stream, compressionMode) : stream;
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
            using (var file = File.Open(GetPath(id) + ".entity", FileMode.Open))
                return (TEntity)new BinaryFormatter().Deserialize(file);
        }

        public byte[] GetFile(Guid id)
        {
            var sw = Stopwatch.StartNew();
            using (Stream fileStream = GetFileStream(GetPath(id), FileMode.Open, CompressionMode.Decompress))
            using (MemoryStream ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                Logger.InfoFormat("Read file id={0} time={1}", id, sw.Elapsed.TotalMilliseconds);
                return ms.ToArray();
            }
        }

        public void Remove(Guid id)
        {
            try
            {
                TEntity entity;
                Logger.InfoFormat("Try remove entity from cache id={0}", id);
                if (CacheEntity.TryRemove(id, out entity))
                {
                    var path = GetPath(id);
                    File.Delete(path + ".entity");
                    Logger.InfoFormat("Ok delete entity id={0}", id);
                    File.Delete(path + ".file");
                    Logger.InfoFormat("Ok delete file id={0}", id);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private string GetPath(Guid id)
        {
            return Path.Combine(folder, id.ToString());
        }
    }
}
