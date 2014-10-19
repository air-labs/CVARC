using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RepairTheStarship;

namespace Level2Client
{
    class Level2ClientForm : Form
    {
        double multiplier=2;

        Level2SensorData sensorData;

        public Level2ClientForm()
        {
            ClientSize = new Size((int)(200 * multiplier), (int)(300 * multiplier));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (sensorData==null) return;
            foreach (var item in sensorData.Map)
            {

            }
        }


    }
}
