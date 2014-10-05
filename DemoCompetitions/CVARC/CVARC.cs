using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVARC.V2;

namespace CVARC
{
    static class CVARC
    {
        static void Error(string error, params string[] args)
        {
            var result=string.Format(error,args);
            MessageBox.Show(result,"CVARC", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        static IWorld Initialize(string[] args)
        {
            var cmd = CommandLineAnalyzer.Analyze(args);
            
            if (cmd.Unnamed.Count!=3)
            {
                throw new Exception("Exactly three unnamed arguments are expected: CVARC.exe Assembly Level Mode");
            }

            var assemblyName = cmd.Unnamed[0] + ".dll";

            

            if (!File.Exists(assemblyName))
            {
                throw new Exception(string.Format("The competitions assembly {0} was not found", assemblyName));
            }

            var ass = Assembly.LoadFrom(assemblyName);
            var list = ass.GetExportedTypes().ToList();
            var competitionsClass = list
                .SingleOrDefault(a => a.IsSubclassOf(typeof(Competitions)) && a.Name == cmd.Unnamed[1]);
            if (competitionsClass == null)
                throw new Exception(string.Format("The level {0} was not found il{1}", cmd.Unnamed[1], cmd.Unnamed[0]));
            var ctor = competitionsClass.GetConstructor(new Type[] { });
            var competitions = ctor.Invoke(new object[] { }) as Competitions;

            if (!Environments.Available.ContainsKey(cmd.Unnamed[2]))
                throw new Exception(string.Format(
                    "Mode {0} is unknown, try one of these: {1}",
                    cmd.Unnamed[2],
                    Environments.Available.Keys.Aggregate((a,b)=>a+", "+b)));

            var environment = Environments.Available[cmd.Unnamed[2]]();
            return competitions.Create(cmd.Named, environment);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var world = Initialize(args);
            var form = new KRForm(world);
            Application.Run(form);
        }
    }
}
