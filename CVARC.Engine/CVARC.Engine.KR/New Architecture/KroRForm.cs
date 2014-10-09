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
        public KroRForm(IWorld world)
        {
            this.world=world;
            ClientSize = new System.Drawing.Size(800, 600);


            scores = new Label();
            scores.BackColor = Color.White;
            scores.Font = new Font("Arial", 18);
            scores.Size = new Size(ClientSize.Width, 30);
            scores.BringToFront();
            Controls.Add(scores);
            
            var engine = world.Engine as KroREngine;
            var control = new DrawerControl(new DirectXFormDrawer(engine.DrawerFactory.GetDirectXScene(), new DrawerSettings { ViewMode = ViewModes.Top }));
            control.Size = new Size(ClientSize.Width,ClientSize.Height-scores.Height);
            control.Location = new Point(0, scores.Height);
            Controls.Add(control);


            world.Scores.ScoresChanged += Scores_ScoresChanged;
            UpdateScores();
            new Thread(RunCompetitions) { IsBackground = true }.Start();

        }

        void RunCompetitions()
        {
            double time = 0;
            double oldTime = 0;
            while (true)
            {
                time = world.Clocks.GetNextEventTime();
                (world.Engine as KroREngine).Updates(oldTime, time);
                world.Clocks.Tick(time);
                oldTime = time;
            }
        }

        void UpdateScores()
        {
            var text = world.Scores
                            .GetAllScores()
                            .Select(z => string.Format("{0}:{1}", z.Item1, z.Item2))
                            .Aggregate((a, b) => a + "            " + b);
            scores.Text = text;
        }

        void Scores_ScoresChanged()
        {
            BeginInvoke(new Action(UpdateScores));

        }
    }
}
