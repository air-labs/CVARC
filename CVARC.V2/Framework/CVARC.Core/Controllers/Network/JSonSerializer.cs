using System;
using System.Collections.Generic;
//using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization.Json;

namespace CVARC.V2
{
    public class JsonSerializer : ISerializer
    {
        ////TODO: There must be concurrent dictionary!!!
        //readonly static Dictionary<Type, DataContractJsonSerializer> Serializers = new Dictionary<Type, DataContractJsonSerializer>();
        //public byte[] Serialize(object obj)
        //{
        //    lock (Serializers)
        //    {
        //        using (var stream = new MemoryStream())
        //        {
        //            Get(obj.GetType()).WriteObject(stream, obj);
        //            return stream.ToArray();
        //        }
        //    }
        //}

        //public object Deserialize(Type type, byte[] bytes)
        //{
        //    lock (Serializers)
        //    {
        //        using (var stream = new MemoryStream(bytes))
        //        {
        //            var serializer=Get(type);
        //            try
        //            {
        //                return serializer.ReadObject(stream);
        //            }
        //            catch(Exception e)
        //            {
        //                throw e;
        //            }
        //            }
        //    }
        //}

        //private DataContractJsonSerializer Get(Type t)
        //{
        //    lock (Serializers)
        //    {
        //        if (!Serializers.ContainsKey(t))
        //            Serializers.Add(t, new DataContractJsonSerializer(t));
        //        return Serializers[t];
        //    }
        //}
        public byte[] Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                new DataContractJsonSerializer(obj.GetType()).WriteObject(stream,obj);
                return stream.ToArray();
            }
        }

        public object Deserialize(Type type, byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return new DataContractJsonSerializer(type).ReadObject(stream);
            }
        }
    }
}