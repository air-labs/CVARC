using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVARC.V2;

namespace CVARC.V2
{
    public static class CVARCProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            var world = Competitions.Create(args);
            var form = new KroRForm(world);
            Application.Run(form);
        }
    }
}
