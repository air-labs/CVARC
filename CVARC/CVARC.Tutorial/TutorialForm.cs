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

namespace CVARC.Tutorial
{
    public partial class TutorialForm : Form
    {
        private readonly RobotBehaviour _robotBehaviour;
        public World World { get; set; }
        private const AnchorStyles AnchorAll = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            PhysicalManager.InitializeEngine(PhysicalEngines.Farseer, Root);
            var factory = new DrawerFactory(Root);
            _robotBehaviour.InitSensors();
            _robotBehaviour.Sensors.ForEach(a =>
                {
                    foreach(var robot in World.Robots)
                    {
                        var sens = a.GetOne(robot, World, factory);
                        robot.Sensors.Add(sens);
                    }
                });
            tableLayoutPanel1.Controls.Add(new DrawerControl(new DirectXFormDrawer(factory.GetDirectXScene(), new DrawerSettings { ViewMode = ViewModes.Top })), 1, 0);
            for (int i = 0; i < World.RobotCount; ++i )
                tableLayoutPanel1.Controls.Add(new DrawerControl(new DirectXFormDrawer(factory.GetDirectXScene(), new DrawerSettings
                {
                    BodyCameraLocation = new Frame3D(30, 0, 30, Angle.FromGrad(-45), Angle.Zero, Angle.Zero),
                    Robot = World.Robots[i].Body,
                    ViewMode = ViewModes.FirstPerson
                })), 0, 0);
            tableLayoutPanel1.Controls.Add(ScoreDisplayControl, 1, 1);
            foreach (Control control in tableLayoutPanel1.Controls)
                control.Anchor = AnchorAll;
            new Thread(() =>
            {
                while (true)
                    MakeCycle(true);
            })
            {
                IsBackground = true
            }.Start();
        }

        protected Body Root { get; set; }

        protected virtual void MakeCycle(bool realtime)
        {
            PhysicalManager.MakeIteration(1.0/10000, Root);
            foreach (Body body in Root)
                body.Update(1/60);
            ScoreDisplayControl.UpdateDisplayedScores();
        }

        public ScoreDisplayControl ScoreDisplayControl;
        public PictureBox BitmapDisplayer;

        public TutorialForm(World world, RobotBehaviour robotBehaviour, Type kb)
        {
            _robotBehaviour = robotBehaviour;
            World = world;
            InitializeComponent();
            ClientSize = new Size(800, 600);
            Root = world.Init();
            ScoreDisplayControl = new ScoreDisplayControl{Scores = world.Score};
            var kbContr = kb.GetConstructor(new[] {typeof(Form)});
            if(kbContr != null)
            {
                var contr = kbContr.Invoke(new object[]{this}) as KeyboardController;
                if(contr != null) contr.ProcessCommand += command => robotBehaviour.ProcessCommand(World.Robots[command.RobotId], command);
            }
        }
    }
}
