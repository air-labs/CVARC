using System;
using System.Linq;
using System.Xml.Linq;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using StarshipRepair;

namespace Gems
{
    public class SRNetworkController : NetworkController
    {
        override public Command ParseRequest(string request)
        {
            var doc = XDocument.Parse(request);
            var content = doc.Elements().Where(z => z.Name.LocalName == "Command").FirstOrDefault();
            if (content == null) throw new Exception("Wrong header of command package, <Command> is expected"); 
            var linearVelocity = GetDouble(content, "LinearVelocity");
            var angularVelocity = GetDouble(content, "AngularVelocity");
            var time = GetDouble(content, "Time");
            var action = GetString(content, "Action");

            if (time < 0) time = 0;
            var command = new Command { Time = time };

            if (Math.Abs(linearVelocity) > SRCompetitions.MaxLinearVelocity) linearVelocity = Math.Sign(linearVelocity) * SRCompetitions.MaxLinearVelocity;
            if (linearVelocity != 0)
                command.Move = linearVelocity;
            else
            {
                if (Math.Abs(angularVelocity) > SRCompetitions.MaxAngularVelocity) angularVelocity = Math.Sign(angularVelocity) * SRCompetitions.MaxAngularVelocity;
                if (angularVelocity != 0)
                    command.Angle = Angle.FromGrad(angularVelocity);
                else
                {
                    if (action != null)
                    {
                        command.Cmd = action;
                        command.Time = 1;
                    }
                }
            }
            return command;
        }
    }
}
