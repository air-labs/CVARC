using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Controllers;

namespace CVARK.Network
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0) return;
            if (!File.Exists(args[0])) return;
            var ass = Assembly.LoadFile(args[0]);
            int port;
            if (args.Length == 1 || !int.TryParse(args[1], out port))
                port = 14000;
            var world = ass.GetExportedTypes().FirstOrDefault(a => a.IsSubclassOf(typeof(World)));
            var beh = ass.GetExportedTypes().FirstOrDefault(a => a.IsSubclassOf(typeof(RobotBehaviour))) ?? typeof(RobotBehaviour);
            var kb = ass.GetExportedTypes().FirstOrDefault(a => a.IsSubclassOf(typeof(NetworkController))) ?? typeof(NetworkController);
            if (world == null) return;
            var constrW = world.GetConstructor(new Type[0]);
            var constrB = beh.GetConstructor(new Type[0]);
            if (constrW == null) return;
            var w = constrW.Invoke(new object[0]) as World;
            var b = constrB == null ? new RobotBehaviour() : constrB.Invoke(new object[0]) as RobotBehaviour;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (sender, eventArgs) => Application.Exit();
            var frm = new TutorialForm(w, b, kb, port) {Opacity = 0, ShowInTaskbar = false};
            frm.Closed += (sender, eventArgs) => Environment.Exit(0);
            Application.Run(frm);
        }
    }
}
