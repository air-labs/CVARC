using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CVARC.Graphics;
using CVARC.Graphics.DirectX;
using System.Threading;

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
            var control = new DrawerControl(new DirectXFormDrawer(engine.DrawerFactory.GetDirectXScene(), new DrawerSettings { ViewMode = ViewModes.Top }));
            control.Size = new Size(ClientSize.Width,ClientSize.Height-scores.Height);
            control.Location = new Point(0, scores.Height);
            Controls.Add(control);


            world.Scores.ScoresChanged += Scores_ScoresChanged;
            UpdateScores();
            thread= new Thread(RunCompetitions) { IsBackground = true };
            thread.Start();

           

        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        bool worldExited = false;

        void RunCompetitions()
        {
            double time = 0;
            double oldTime = 0;
            while (true)
            {
                time = world.Clocks.GetNextEventTime();
                if (time > world.Clocks.TimeLimit) break;
                (world.Engine as KroREngine).Updates(oldTime, time);
                world.Clocks.Tick(time);
                oldTime = time;
                BeginInvoke(new Action(UpdateClocks));
            }
            world.OnExit();
            worldExited = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            thread.Abort();
            if (!worldExited)
                world.OnExit();
        }


        void UpdateScores()
        {
            var text = world.Scores
                            .GetAllScores()
                            .Select(z => string.Format("{0}:{1}", z.Item1, z.Item2))
                            .Aggregate((a, b) => a + "            " + b);
            scores.Text = text;
        }

        void UpdateClocks()
        {
            clocks.Text = Math.Round(world.Clocks.CurrentTime).ToString();
        }

        void Scores_ScoresChanged()
        {
            BeginInvoke(new Action(UpdateScores));

        }
    }
}
