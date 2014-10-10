using CVARC.V2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogComparator
{
    class Program
    {
        static double Compare(Log l1, Log l2)
        {
            var result = 0.0;
            foreach (var e in l1.Positions.Keys)
            {
                if (!l2.Positions.ContainsKey(e)) throw new Exception();
                if (l1.Positions[e].Count != l2.Positions[e].Count) throw new Exception();
                for (int i = 0; i < l1.Positions[e].Count; i++)
                {
                    var e1 = l1.Positions[e][i];
                    var e2 = l2.Positions[e][i];
                    if (e1.Time != e2.Time) throw new Exception();
                    if (e1.Exists != e2.Exists) throw new Exception();
                    if (!e1.Exists) continue;
                    result = Math.Max(result, Math.Abs(e1.Location.X - e2.Location.X));
                    result = Math.Max(result, Math.Abs(e1.Location.Y - e2.Location.Y));
                    result = Math.Max(result, Math.Abs(e1.Location.Yaw.Radian - e2.Location.Yaw.Radian));
                }
            }
            return result;
        }

        static void Main(string[] args)
        {
            var directory = new DirectoryInfo("..\\..\\Logs\\");
            var files = directory.GetFiles("*.cvarclog");
            var original = Log.Load(files[0].FullName);
            var result = 0.0;
            foreach (var file in files.Skip(1))
            {
                var log = Log.Load(file.FullName);
                result = Math.Max(result, Compare(original, log));
            }
            Console.WriteLine(result);
        }
    }
}
