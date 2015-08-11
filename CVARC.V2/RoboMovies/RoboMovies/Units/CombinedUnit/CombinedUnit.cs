using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using RoboMovies;

namespace CVARC.V2
{
    public class CombinedUnit : IUnit
    {
        public IActor Actor;
        public Dictionary<string, Func<IActor, double>> SubUnits;

        public CombinedUnit(IActor actor)
        {
            this.Actor = actor;
            this.SubUnits = new Dictionary<string, Func<IActor, double>>();
        }

        public UnitResponse ProcessCommand(object _command)
        {
            var command = Compatibility.Check<ICombinedCommand>(this, _command).CombinedCommand;
            
            Debugger.Log(RMDebugMessage.Workflow, "Command comes to combined unit");

            if (command == null || command.Length == 0)
                return UnitResponse.Denied();

            var accessCount = 0;
            var maxTime = 0.0;

            foreach (var e in command)
            {
                if (SubUnits.ContainsKey(e))
                {
                    var currentTime = SubUnits[e](Actor);
                    if (currentTime > maxTime)
                        maxTime = currentTime;
                    accessCount++;
                }
            }
            if (accessCount == 0) return UnitResponse.Denied();
            return UnitResponse.Accepted(maxTime);
        }
    }
}
