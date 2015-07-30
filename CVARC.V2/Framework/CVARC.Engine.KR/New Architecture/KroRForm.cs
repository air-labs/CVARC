using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CVARC.Graphics;
using CVARC.Graphics.DirectX;
using System.Threading;
using System.IO;

namespace CVARC.V2
{
    public class KroRForm : Form
    {
        double clock = 0;
        IWorld world;
        Label scores;
        Label clocks;
        Thread thread;
        public KroRForm(IWorld world)
        {
            this.world=world;
            ClientSize = new System.Drawing.Size(800, 600);

            var font=new Font("Arial", 18);

            clocks=new Label();
            clocks.Size=new Size(100,50);
            clocks.Location=new Point(ClientSize.Width-clocks.Width,0);
            clocks.Font=font;
            clocks.BackColor=Color.White;
            Controls.Add(clocks);

            scores = new Label();
            scores.BackColor = Color.White;
            scores.Font = font;
            scores.Size = new Size(ClientSize.Width-clocks.Width, clocks.Height);
            scores.BringToFront();
            Controls.Add(scores);
            
            var engine = world.Engine as KroREngine;
            var control = new DrawerControl(new DirectXFormDrawer(engine.DrawerFactory.GetDirectXScene(), new DrawerSettings
            {
                ViewMode = ViewModes.FirstPerson,
                BodyCameraLocation = world.Configuration.Settings.ObserverCameraLocation,
                Robot = engine.Root
            }));



            control.Size = new Size(ClientSize.Width,ClientSize.Height-scores.Height);
            control.Location = new Point(0, scores.Height);
            Controls.Add(control);


            world.Scores.ScoresChanged += () => { Invoke(new Action(UpdateScores)); };
            world.Clocks.Ticked += () => { Invoke(new Action(UpdateClocks)); };
            world.Exit += () => worldExited = true;

            UpdateScores();
            thread= new Thread(()=>world.RunActively(1)) { IsBackground = true };
            thread.Start();

           

        }

        bool worldExited = false;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (!worldExited)
                world.OnExit();
            thread.Abort();
        }


        void UpdateScores()
        {
            var sc = world.Scores
                            .GetAllScores()
                            .Select(z => string.Format("{0}:{1}", z.Item1, z.Item2))
                            .ToArray();
            if (sc.Length==0) 
                scores.Text="";
            else scores.Text=sc.Aggregate((a, b) => a + "            " + b);
        }

        void UpdateClocks()
        {
            clocks.Text = Math.Round(world.Clocks.CurrentTime).ToString();
        }
    }
}
