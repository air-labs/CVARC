using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RepairTheStarship;

namespace Level1Example
{
    class Program
    {
        static void DebugRun(Action action)
        {
            new Action(() =>
                {
                    Thread.Sleep(1000);
                    action();
                }).BeginInvoke(null, null);
            CVARCProgram.Main(new string[] { "-Debug", "-Port", "14000" });
        }

        static void Control()
        {
            var client = new Level1Client();
            client.Configurate(true, RepairTheStarshipBots.Azura);
        //    client.Rotate(-90);
        //    client.Move(100);
        //    client.Exit();
        }

        [STAThread]
        public static void Main()
        {
            DebugRun(Control);
        }
    }
}
