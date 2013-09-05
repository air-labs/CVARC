using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Basic.Sensors;
using CVARC.Core;
using CVARC.Graphics;
using CVARC.Graphics.DirectX;
using CVARC.Physics;

namespace CVARC.Basic
{


    public partial class TutorialForm : Form
    {
        private const AnchorStyles AnchorAll = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        public ScoreDisplayControl ScoreDisplayControl;
        public PictureBox BitmapDisplayer;

        public void UpdateScores()
        {
            ScoreDisplayControl.UpdateDisplayedScores();
        }

        public TutorialForm(Competitions competitions)
        {
            InitializeComponent();
            ClientSize = new Size(800, 600);
            ScoreDisplayControl = new ScoreDisplayControl{Scores = competitions.World.Score};
            tableLayoutPanel1.Controls.Add(new DrawerControl(new DirectXFormDrawer(competitions.DrawerFactory.GetDirectXScene(), new DrawerSettings { ViewMode = ViewModes.Top })), 1, 0);
            for (int i = 0; i < competitions.World.RobotCount; ++i)
                tableLayoutPanel1.Controls.Add(new DrawerControl(new DirectXFormDrawer(competitions.DrawerFactory.GetDirectXScene(), new DrawerSettings
                {
                    BodyCameraLocation = new Frame3D(30, 0, 30, Angle.FromGrad(-45), Angle.Zero, Angle.Zero),
                    Robot = competitions.World.Robots[i].Body,
                    ViewMode = ViewModes.FirstPerson
                })), 0, 0);
            tableLayoutPanel1.Controls.Add(ScoreDisplayControl, 1, 1);
            foreach (Control control in tableLayoutPanel1.Controls)
                control.Anchor = AnchorAll;
            competitions.CycleFinished += (s, a) => BeginInvoke(new Action(UpdateScores));
        }
    }
}
