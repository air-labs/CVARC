using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CameraClient
{
    class CameraClientForm : Form
    {
        PictureBox box;

        public void UpdateBitmap(Bitmap bitmap)
        {
            BeginInvoke(new Action<Bitmap>(ThreadSafeUpdateBitmap), new object[] { bitmap });
        }

        void ThreadSafeUpdateBitmap(Bitmap bitmap)
        {
            box.Image = bitmap;
            ClientSize = bitmap.Size;
        }

        public CameraClientForm()
        {
            box = new PictureBox();
            box.Dock = DockStyle.Fill;
            Controls.Add(box);
        }
    }
}
