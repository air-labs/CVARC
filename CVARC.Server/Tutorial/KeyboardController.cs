using System.Windows.Forms;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;

namespace CVARC.Tutorial
{
    public class KeyboardController
    {
        public Command GetCommand(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.W:
                    return new Command {Time = 10, LinearVelocity = 100, RobotId = 0};
                case Keys.S:
                    return new Command {Time = 10, LinearVelocity = -100, RobotId = 0};
                case Keys.A:
                    return new Command {Time = 10, AngularVelocity = Angle.FromGrad(90), RobotId = 0};
                case Keys.D:
                    return new Command {Time = 10, AngularVelocity = Angle.FromGrad(-90), RobotId = 0};
                case Keys.Q:
                    return new Command {Action = CommandAction.Grip, RobotId = 0};
                case Keys.E:
                    return new Command {Action = CommandAction.Release, RobotId = 0};
                case Keys.NumPad8:
                    return new Command {LinearVelocity = 100, RobotId = 1};
                case Keys.NumPad5:
                    return new Command {LinearVelocity = -100, RobotId = 1};
                case Keys.NumPad4:
                    return new Command {AngularVelocity = Angle.FromGrad(90), RobotId = 1};
                case Keys.NumPad6:
                    return new Command {AngularVelocity = Angle.FromGrad(-90), RobotId = 1};
                case Keys.NumPad7:
                    return new Command {Action = CommandAction.Grip, RobotId = 1};
                case Keys.NumPad9:
                    return new Command {Action = CommandAction.Release, RobotId = 1};
                default:
                    return Command.Sleep();
            }
        }
    }
}
