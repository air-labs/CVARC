using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;

namespace CVARC.V2
{
    public class IdGenerator
    {
        Dictionary<string, object> idToKey = new Dictionary<string, object>();
        Dictionary<object, int> keyIdCount = new Dictionary<object, int>();

        public string CreateNewId(object obj)
        {
            if (!keyIdCount.ContainsKey(obj)) keyIdCount[obj] = 0;
            var id = Combine(obj.GetType().Name, obj.ToString(), keyIdCount[obj].ToString());

            if (idToKey.ContainsKey(id)) //two key object has the same type and does not override ToString method
            {
                for (int i = 0; ; i++)
                {
                    var newId = Combine(id, i.ToString());
                    if (!idToKey.ContainsKey(newId))
                    {
                        id = newId;
                        break;
                    }
                }
            }

            keyIdCount[obj]++;
            idToKey[id] = obj;
            return id;
        }

        public object GetKey(string id)
        {
            if (!idToKey.ContainsKey(id)) throw new ArgumentException("Unrecognized object ID");
            return idToKey[id];
        }

        public TKey GetKey<TKey>(string id)
        {
            var key = GetKey(id);
            if (!(key is TKey)) throw new Exception("The key is not of type TKey");
            return (TKey)key;
        }

        public bool KeyOfType<TKey>(string id)
        {
            if (!idToKey.ContainsKey(id)) throw new ArgumentException("Unrecognized object ID");
            return idToKey[id] is TKey;
        }

        public IEnumerable<Tuple<object, string>> GetAllPairs()
        {
            return idToKey.Select(z => new Tuple<object, string>(z.Value, z.Key));
        }

        public IEnumerable<Tuple<T, string>> GetAllPairsOfType<T>()
        {
            return GetAllPairs().Where(z => z.Item1 is T).Select(z => new Tuple<T, string>((T)z.Item1, z.Item2));
        }


        public IEnumerable<string> GetAllId()
        {
            return idToKey.Keys;
        }

        public static string Combine(params string[] values)
        {
            return values.Aggregate((a, b) => a + "." + b);
        }

    }
}
