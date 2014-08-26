using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CVARC.Basic.Core.Serialization
{
    public class JsonSerializer : ISerializer
    {
        readonly static ConcurrentDictionary<Type, DataContractJsonSerializer> Serializers = new ConcurrentDictionary<Type, DataContractJsonSerializer>();
        public byte[] Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                Get(obj.GetType()).WriteObject(stream, obj);
                return stream.ToArray();
            }
        }

        public T Deserialize<T>(byte[] bytes)
        {
            var s = Encoding.UTF8.GetString(bytes);
            using(var stream = new MemoryStream(bytes))
                return (T) Get(typeof(T)).ReadObject(stream);
        }

        private DataContractJsonSerializer Get(Type t)
        {
            return new DataContractJsonSerializer(t);
            return Serializers.GetOrAdd(t, type => new DataContractJsonSerializer(type));
        }
    }
}