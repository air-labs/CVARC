using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalNetworkClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Process();
            p.StartInfo.FileName = "..\\..\\..\\..\\CVARC\\CVARC.Network\\bin\\Debug\\CVARC.Network.exe";
            p.StartInfo.Arguments= "..\\..\\..\\..\\Competitions\\Fall2013.0\\bin\\Debug\\Fall2013.0.dll";
            p.Start();
        }
    }
}
