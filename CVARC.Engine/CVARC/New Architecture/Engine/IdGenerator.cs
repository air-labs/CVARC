using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            keyIdCount[obj]++;
            idToKey[id] = obj;
            return id;
        }

        public object GetKey(string id)
        {
            if (!idToKey.ContainsKey(id)) throw new ArgumentException("Unrecognized object ID");
            return idToKey[id];
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
