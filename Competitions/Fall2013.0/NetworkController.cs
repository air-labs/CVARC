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

            if (Math.Abs(LinearVelocity) > 10) LinearVelocity = Math.Sign(LinearVelocity) * 10;
            if (LinearVelocity != 0)
                command.Move = LinearVelocity;
            else
            {
                if (Math.Abs(AngularVelocity) > 20)  AngularVelocity = Math.Sign(AngularVelocity) * 20;
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
