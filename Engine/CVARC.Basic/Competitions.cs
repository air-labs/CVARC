using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using CVARC.Basic.Controllers;

namespace CVARC.Basic
{
    public class Competitions
    {
        public readonly World World;
        public readonly RobotBehaviour Behaviour;
        public readonly KeyboardController KeyboardController;
        public Competitions(World world, RobotBehaviour behaviour, KeyboardController keyboard)
        {
            World = world;
            Behaviour = behaviour;
            KeyboardController = keyboard;
        }

        public static Competitions Load(string filename)
        {
            if (!File.Exists(filename))
            {
                MessageBox.Show("The assembly file you specified does not exist", "CVARC Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            var file = new FileInfo(filename);
            var ass = Assembly.LoadFile(file.FullName);
            var competitions = ass.GetExportedTypes().FirstOrDefault(a => a.IsSubclassOf(typeof(Competitions)));
            var ctor = competitions.GetConstructor(new Type[] { });
            return ctor.Invoke(new object[] { }) as Competitions;
        }
    }
}
