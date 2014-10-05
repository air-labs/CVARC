using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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
        public KroRForm(IWorld world)
        {
            this.world=world;
            var engine = world.Engine as KroREngine;
            ClientSize = new System.Drawing.Size(800, 600);
            var control = new DrawerControl(new DirectXFormDrawer(engine.DrawerFactory.GetDirectXScene(), new DrawerSettings { ViewMode = ViewModes.Top }));
            control.Dock = DockStyle.Fill;
            Controls.Add(control);

            timer = new Timer();
            timer.Interval = 1;
            timer.Tick += TimerTick;
            timer.Start();

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
