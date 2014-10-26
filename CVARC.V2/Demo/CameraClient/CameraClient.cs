using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
using Demo;

namespace CameraClient
{
    public class Program
    {
        static CameraClientForm form;

        static void ControlAndShow(bool runServer)
        {
            form = new CameraClientForm();
            new Action<bool>(Control).BeginInvoke(runServer, null, null);
            Application.Run(form);
        }

        static void Control(bool runServer)
        {
            var client = new CvarcClient<SensorsWithCamera, SimpleMovementCommand>();
            client.Configurate(runServer, 14000, new ConfigurationProposal
            {
                LoadingData = new LoadingData { AssemblyName = "Demo", Level = "Camera" },
                SettingsProposal = new SettingsProposal
                {
                    Controllers = new List<ControllerSettings>
                      {
                          new ControllerSettings { ControllerId="Left", Type= ControllerType.Client, Name="This" },
                          new ControllerSettings { ControllerId="Right", Type= ControllerType.Bot, Name="Random" }
                      }
                }
            });
            for (int i = 0; i < 100; i++)
            {
                var sensorsData = client.Act(SimpleMovementCommand.Rotate(Angle.Pi/20,0.1));
                var stream = new MemoryStream(sensorsData.Image);
                var bitmap = (Bitmap)Bitmap.FromStream(stream);
                form.UpdateBitmap(bitmap);
            }
        }

        public static void Main(string[] args)
        {
            CVARC.V2.CVARCProgram.RunServerInTheSameThread(args, ControlAndShow);
        }
    }
}
