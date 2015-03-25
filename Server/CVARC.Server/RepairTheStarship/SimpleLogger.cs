using System;
using System.IO;
using System.Windows.Forms;

namespace Gems
{
    public class SimpleLogger
    {
        private const string FolderPath = "../../../Cvarc.Logs/";

        public static void Run()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, a) =>
                {
                    if (!Directory.Exists(FolderPath))
                        Directory.CreateDirectory(FolderPath);
                    File.AppendAllText(FolderPath + "cvarc.log", a.ExceptionObject.ToString());
                    MessageBox.Show(a.ExceptionObject.ToString(), "CVARC Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                };
        }
    }
}
