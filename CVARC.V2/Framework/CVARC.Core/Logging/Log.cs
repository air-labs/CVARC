using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CVARC.V2
{
    [Serializable]
    public class Log
    {
        public Configuration Configuration { get; set; }

        public readonly Dictionary<string, List<PositionLogItem>> Positions = new Dictionary<string, List<PositionLogItem>>();

        public void Save(string filename)
        {
            using (var stream = File.Open(filename, FileMode.Create, FileAccess.Write))
            {
                new BinaryFormatter().Serialize(stream, this);
            }
        }

        public readonly Dictionary<string, List<ICommand>> Commands = new Dictionary<string, List<ICommand>>();

        public IWorldState WorldState { get; set; }

        public static Log Load(string filename)
        {
            using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                return (Log)(new BinaryFormatter().Deserialize(stream));
            }
        }
    }
}
