using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Controllers;

namespace Gems
{
    public class GemsNetworkController : NetworkController
    {
        public const double MaxLinearVelocity = 30;
        public const double MaxAngularVelocity = 30;

        override public Command ParseRequest(string request)
        {
            var doc = XDocument.Parse(request);

            var content = doc.Elements().Where(z => z.Name.LocalName == "Command").FirstOrDefault();
            if (content == null) throw new Exception("Wrong header of command package, <Command> is expected"); 
            var LinearVelocity = GetDouble(content, "LinearVelocity");
            var AngularVelocity = GetDouble(content, "AngularVelocity");
            var Time = GetDouble(content, "Time");
            var Action = GetString(content, "Action");


            if (Time < 0) Time = 0;
            var command = new Command { Time = Time };

            if (Math.Abs(LinearVelocity) > MaxLinearVelocity) LinearVelocity = Math.Sign(LinearVelocity) * MaxLinearVelocity;
            if (LinearVelocity != 0)
                command.Move = LinearVelocity;
            else
            {
                if (Math.Abs(AngularVelocity) > MaxAngularVelocity) AngularVelocity = Math.Sign(AngularVelocity) * MaxAngularVelocity;
                if (AngularVelocity != 0)
                    command.Angle = Angle.FromGrad(AngularVelocity);
                else
                {
                    if (Action != null)
                    {
                        command.Cmd = Action;
                        command.Time = 1;
                    }
                }
            }

            return command;
        }

    }
}
