using System.Collections.Generic;
using System.Windows.Forms;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;

namespace Gems
{
    public class KbController : KeyboardController
    {
        public KbController()
        {
        }

        public override IEnumerable<Command> GetCommand(Keys keyData)
        {
            switch(keyData)
            {
                case Keys.W:
                    yield return new Command{ Move = 100, RobotId = 0};
                    break;
                case Keys.S:
                    yield return new Command { Move = -100, RobotId = 0 };
                    break;
                case Keys.A:
                    yield return new Command { Angle = Angle.FromGrad(90), RobotId = 0 };
                    break;
                case Keys.D:
                    yield return new Command { Angle = Angle.FromGrad(-90), RobotId = 0 };
                    break;
                case Keys.R:
                    yield return new Command { Cmd = "Grip", RobotId = 0 };
                    break;
                case Keys.F:
                    yield return new Command { Cmd = "Release", RobotId = 0 };
                    break;
                case Keys.I:
                    yield return new Command { Move = 100, RobotId = 1 };
                    break;
                case Keys.K:
                    yield return new Command { Move = -100, RobotId = 1 };
                    break;
                case Keys.J:
                    yield return new Command { Angle = Angle.FromGrad(90), RobotId = 1 };
                    break;
                case Keys.L:
                    yield return new Command { Angle = Angle.FromGrad(-90), RobotId = 1 };
                    break;

                case Keys.P:
                    yield return new Command { Cmd = "Grip", RobotId = 1 };
                    break;
                case Keys.OemSemicolon:
                    yield return new Command { Cmd = "Release", RobotId = 1 };
                    break;
            }
        }
    }
}