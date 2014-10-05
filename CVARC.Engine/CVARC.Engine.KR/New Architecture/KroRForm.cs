using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CVARC.Graphics;
using CVARC.Graphics.DirectX;

namespace CVARC.V2
{
    public class KroRForm : Form
    {
        double clock = 0;
        IWorld world;
        Timer timer;
        Label scores;
        public KroRForm(IWorld world)
        {
            this.world=world;
            ClientSize = new System.Drawing.Size(800, 600);


            scores = new Label();
            scores.BackColor = Color.White;
            scores.Font = new Font("Arial", 18);
            scores.Size = new Size(200, 30);
            scores.BringToFront();
            Controls.Add(scores);
            
            var engine = world.Engine as KroREngine;
            var control = new DrawerControl(new DirectXFormDrawer(engine.DrawerFactory.GetDirectXScene(), new DrawerSettings { ViewMode = ViewModes.Top }));
            control.Size = new Size(ClientSize.Width,ClientSize.Height-scores.Height);
            control.Location = new Point(0, scores.Height);
            Controls.Add(control);



            timer = new Timer();
            timer.Interval = 1;
            timer.Tick += TimerTick;
            timer.Start();

            world.Scores.ScoresChanged += Scores_ScoresChanged;

        }

        void Scores_ScoresChanged()
        {
            var text = world.Scores
                .GetAllScores()
                .Select(z => string.Format("{0}:{1}", z.Item1, z.Item2))
                .Aggregate((a, b) => a + "            " + b);
            scores.Text = text;
        }


        void TimerTick(object sender, EventArgs e)
        {
            world.Clocks.Tick(clock);
            var nextCall = world.Clocks.GetNextEventTime();
            clock = nextCall;
            var interval=(int)(1000 * (nextCall - world.Clocks.CurrentTime));
            if (interval==0) interval=1;
            timer.Interval = interval;
        }
    }
}
