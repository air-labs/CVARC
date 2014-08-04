using System;
using System.Collections.Generic;
//using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization.Json;

namespace CVARC.Basic.Core.Serialization
{
    public class JsonSerializer : ISerializer
    {
        //TODO: There must be concurrent dictionary!!!
        readonly static Dictionary<Type, DataContractJsonSerializer> Serializers = new Dictionary<Type, DataContractJsonSerializer>();
        public byte[] Serialize(object obj)
        {
            lock (Serializers)
            {
                using (var stream = new MemoryStream())
                {
                    Get(obj.GetType()).WriteObject(stream, obj);
                    return stream.ToArray();
                }
            }
        }

        public T Deserialize<T>(byte[] bytes)
        {
            lock (Serializers)
            {
                using (var stream = new MemoryStream(bytes))
                    return (T)Get(typeof(T)).ReadObject(stream);
            }
        }

        private DataContractJsonSerializer Get(Type t)
        {
            lock (Serializers)
            {
                if (!Serializers.ContainsKey(t))
                    Serializers.Add(t, new DataContractJsonSerializer(t));
                return Serializers[t];
            }
        }
    }
}