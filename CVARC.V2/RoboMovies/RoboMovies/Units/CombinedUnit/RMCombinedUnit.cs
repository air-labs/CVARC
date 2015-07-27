using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using RoboMovies;

namespace CVARC.V2
{
    public class RMCombinedUnit : CombinedUnit
    {
        Frame3D LeftClapperPoint;
        Frame3D RightClapperPoint;
        Frame3D LadderMoverPoint;

        public Func<IEnumerable<string>> FindClapperboards;

        public RMCombinedUnit(IActor actor) :
            base(actor)
        {
            LeftClapperPoint = new Frame3D(0,0,0);
            RightClapperPoint = new Frame3D(0,0,0);
            LadderMoverPoint = new Frame3D(0,0,0);

            SubUnits.Add("LeftDeployer", x => MakeClapper(x, LeftClapperPoint));
            SubUnits.Add("RightDeployer", x => MakeClapper(x, RightClapperPoint));
            SubUnits.Add("LadderTaker", x => GoLadder(x, LadderMoverPoint));
        }

        private double Distance(Frame3D a, Frame3D b)
        {
            return AIRLab.Mathematics.Geometry.Distance(a.ToPoint3D(), b.ToPoint3D());
        }

        public double MakeClapper(IActor actor, Frame3D point)
        {
            var clapperId = FindClapperboards();
            var clapperboardsList = clapperId
                .Select(z => new { Id = z, Place = actor.World.Engine.GetAbsoluteLocation(z) })
                .Select(z => new { Id = z.Id, Dist = Distance(point, z.Place) }) //заменить Distance на встроенную
                .OrderBy(z => z.Dist)
                .ToList();
            if (clapperboardsList.Count == 0) return 1;
            if (clapperboardsList[0].Dist > 30) return 1;
            var clapperToDeploy = clapperboardsList[0].Id;

            actor.World.Scores.Add(actor.ControllerId, 10, "clapperboardDeploying");

            return 1;
        }

        public double GoLadder(IActor actor, Frame3D point)
        {
            return double.PositiveInfinity;
        }
    }
}
