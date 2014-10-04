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
    public class KRForm : Form
    {
        double clock = 0;
        double dt = 0.01;
        IWorld world;
        public KRForm(IWorld world)
        {
            this.world=world;
            var engine = world.Engine.Physical as KRPhysical;
            ClientSize = new System.Drawing.Size(800, 600);
            var control = new DrawerControl(new DirectXFormDrawer(engine.DrawerFactory.GetDirectXScene(), new DrawerSettings { ViewMode = ViewModes.Top }));
            control.Dock = DockStyle.Fill;
            Controls.Add(control);

            var timer = new Timer();
            timer.Interval = (int)(dt * 1000);
            timer.Tick += TimerTick;
            timer.Start();

        }

        void TimerTick(object sender, EventArgs e)
        {
            world.Tick(clock);
            clock += dt;
        }
    }
}
