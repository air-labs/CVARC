using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RepairTheStarship;

namespace ClientExample
{
    class ClientForm : Form
    {
        float multiplier=1;

        Map map;

        public ClientForm()
        {
            ClientSize = new Size((int)(2*300 * multiplier), (int)(2*200 * multiplier));
        }

        Color DetailColor(DetailColor color)
        {
            switch (color)
            {
                case RepairTheStarship.DetailColor.Blue: return Color.Blue;
                case RepairTheStarship.DetailColor.Green: return Color.Green;
                case RepairTheStarship.DetailColor.Red: return Color.Red;
            }
            throw new ArgumentException();
        }

        Color WallColor(WallSettings wall)
        {
            switch (wall)
            {
                case WallSettings.Wall: return Color.DarkGray;
                case WallSettings.RedSocket: return Color.DarkRed;
                case WallSettings.GreenSocket: return Color.DarkGreen;
                case WallSettings.BlueSocket: return Color.DarkBlue;
            }
            throw new ArgumentException();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (map==null) return;
            var graphics = e.Graphics;
            graphics.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
            graphics.ScaleTransform(multiplier, -multiplier);
            foreach (var item in map.Details)
            {
                graphics.FillEllipse(
                    new SolidBrush(DetailColor(item.Color)),
                    item.Location.X  - 5,
                    item.Location.Y  - 5,
                    10, 10);
            }
            foreach (var item in map.Walls)
            {
                graphics.DrawLine(
                    new Pen(WallColor(item.Type), 5),
                    item.FirstEnd.X ,
                    item.FirstEnd.Y ,
                    item.SecondEnd.X ,
                    item.SecondEnd.Y );
            }
        }

        public void ShowMap(Map map)
        {
            this.map = map;
            Invalidate();
        }

    }
}
