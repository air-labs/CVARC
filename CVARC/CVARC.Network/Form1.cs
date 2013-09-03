using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Core;
using CVARC.Core.Replay;
using CVARC.Graphics;
using CVARC.Graphics.DirectX;
using CVARC.Physics;

namespace CVARK.Network
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
                foreach (var robot in World.Robots)
                {
                    var sens = a.GetOne(robot, World, factory);
                    robot.Sensors.Add(sens);
                }
            });
            tableLayoutPanel1.Controls.Add(new DrawerControl(new DirectXFormDrawer(factory.GetDirectXScene(), new DrawerSettings { ViewMode = ViewModes.Top })), 1, 0);
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

        private ReplayLogger logger;
        protected Body Root { get; set; }

        protected virtual void MakeCycle(bool realtime)
        {
            try
            {
                PhysicalManager.MakeIteration(1.0/10000, Root);
                foreach (Body body in Root)
                    body.Update(1/60);
                ScoreDisplayControl.UpdateDisplayedScores();
                logger.LogBodies();
            }
            catch (Exception e)
            {
                Application.Exit();
            }
        }

        public ScoreDisplayControl ScoreDisplayControl = new ScoreDisplayControl { Scores = new ScoreCollection(2) };
        public PictureBox BitmapDisplayer;

        public TutorialForm(World world, RobotBehaviour robotBehaviour, Type kb, int port)
        {   
            _robotBehaviour = robotBehaviour;
            World = world;
            InitializeComponent();
            ClientSize = new Size(800, 600);
            Root = world.Init();
            logger = new ReplayLogger(Root, 1);
            ScoreDisplayControl = new ScoreDisplayControl { Scores = World.Score };
            var kbContr = kb.GetConstructor(new []{typeof(World)});
            if (kbContr != null)
            {
                var contr = kbContr.Invoke(new object[]{World}) as NetworkController;
                if (contr != null)
                {
                    contr.Logger = logger;
                    contr.ProcessCommand += command =>
                                            robotBehaviour.ProcessCommand(World.Robots[command.RobotId], command);
                    new Thread(() => contr.Run(port)).Start();
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}
