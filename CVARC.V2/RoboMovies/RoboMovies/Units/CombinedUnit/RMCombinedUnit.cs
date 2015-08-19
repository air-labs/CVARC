using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using RoboMovies;
using AIRLab;

namespace CVARC.V2
{
    public class RMCombinedUnit : CombinedUnit
    {
        const int claperBoardScore = 5;
        const int stairsScore = 15;

        Frame3D LeftClapperOffset;
        Frame3D RightClapperOffset;

        RMWorld world;

        public RMCombinedUnit(IActor actor, RMWorld world) :
            base(actor)
        {
            LeftClapperOffset = new Frame3D(0, 12, 6);
            RightClapperOffset = new Frame3D(0, -12, 6);

            this.world = world;

            SubUnits.Add("LeftDeployer", x => MakeClapper(x, LeftClapperOffset));
            SubUnits.Add("RightDeployer", x => MakeClapper(x, RightClapperOffset));
            SubUnits.Add("LadderTaker", x => GoStair(x));
            SubUnits.Add("PopcornMachineUser", x => TakePopCorn(x));
        }

        private double Distance(Frame3D a, Frame3D b)
        {
            return AIRLab.Mathematics.Geometry.Distance(a.ToPoint3D(), b.ToPoint3D());
        }

        private IEnumerable<Tuple<RMObject, string>> GetObjectsOfType(IActor actor, ObjectType objectType)
        {
            return actor.World.IdGenerator.GetAllPairsOfType<RMObject>()
                .Where(z => z.Item1.Type == objectType);
        }

        private Tuple<string, SideColor> GetClapperboardToDeploy(IActor actor, Frame3D deployerLocation)
        {
            var clapperboards = GetObjectsOfType(actor, ObjectType.Clapperboard)
                .Select(z => new
                {
                    Id = z.Item2,
                    Dist = Distance(deployerLocation, actor.World.Engine.GetAbsoluteLocation(z.Item2)),
                    Color = z.Item1.Color
                }).ToList();

            Debugger.Log(RMDebugMessage.Logic, String.Format("Found {0} clapperboards:", clapperboards.Count));

            clapperboards = clapperboards
                .Where(z => z.Dist < 10)
                .Where(z => !((RMWorld)actor.World).ClosedClapperboards.Contains(z.Id))
                .ToList();

            Debugger.Log(RMDebugMessage.Logic, String.Format("{0} clapperboard(s) available", clapperboards.Count));

            if (clapperboards.Count == 0) return null;
            return new Tuple<string, SideColor>(clapperboards[0].Id, clapperboards[0].Color);
        }

        private Frame3D GetDirectionFrame(Frame3D locationFrame)
        {
            return new Frame3D(0, 0, 0, locationFrame.Pitch, locationFrame.Yaw, locationFrame.Roll);
        }

        public double MakeClapper(IActor actor, Frame3D deployerOffset)
        {
            Debugger.Log(RMDebugMessage.Logic, "Closing clapperboard");

            var actorLocation = actor.World.Engine.GetAbsoluteLocation(actor.ObjectId);
            var deployerLocation = GetDirectionFrame(actorLocation).Apply(deployerOffset) + actorLocation;

            var target = GetClapperboardToDeploy(actor, deployerLocation);
            if (target == null)
            {
                Debugger.Log(RMDebugMessage.Logic, "Clapperboard not found!");
                return 1;
            }

            world.Manager.CloseClapperboard(target.Item1);
            world.ClosedClapperboards.Add(target.Item1);

            SolveClapperboardScores(actor, target.Item2 == SideColor.Yellow ? TwoPlayersId.Left : TwoPlayersId.Right);

            Debugger.Log(RMDebugMessage.Logic, "Clapperboard closed");
            return 1;
        }

        void SolveClapperboardScores(IActor actor, string sideId)
        {
            world.Scores.Add(sideId, claperBoardScore, "Clapperboard of appropriate color has been closed.");
            if (sideId != actor.ControllerId)
                actor.World.Scores.Add(actor.ControllerId, -10, "Closed opponent's clapperboard");
        }

        private bool IsPossibleGoUp(Frame3D bot, Frame3D stair)
        {
            return Math.Abs(bot.X - stair.X) < 20 && Math.Abs(bot.Y - stair.Y) < 100;
        }

        private string GetStairId(IActor actor)
        {
            if (!(TwoPlayersId.Ids.Contains(actor.ControllerId)))
                throw new InvalidProgramException("неизвестный ControllerId");
            var actorSideColor = actor.ControllerId == TwoPlayersId.Left ? SideColor.Yellow : SideColor.Green;
            var allStairs = GetObjectsOfType(actor, ObjectType.Stairs).ToList();
            var StairToGoUp = allStairs
                .Where(z => IsPossibleGoUp(
                        actor.World.Engine.GetAbsoluteLocation(actor.ObjectId),
                        actor.World.Engine.GetAbsoluteLocation(z.Item2))
                    )
                .FirstOrDefault(z => z.Item1.Color == actorSideColor);
            if (StairToGoUp == null) return null;
            return StairToGoUp.Item2;
        }

        public double GoStair(IActor actor)
        {
            var stairId = GetStairId(actor);
            if (stairId != null)
            {
                //синий робот похоже тоже приаттачится к желтой лесенке
                (actor.World.Manager as IRMWorldManager).ClimbUpStairs(actor.ObjectId, stairId);
                actor.World.Scores.Add(actor.ControllerId, stairsScore, "Stairs complete.");
                return Double.PositiveInfinity;
            }
            return 1;
        }

        private string GetMachineId(IActor actor)
        {
            if (!(TwoPlayersId.Ids.Contains(actor.ControllerId)))
                throw new InvalidProgramException("неизвестный ControllerId");
            var sideColor = actor.ControllerId == TwoPlayersId.Left ? SideColor.Yellow : SideColor.Green;
            var allPopCornDispensers = GetObjectsOfType(actor, ObjectType.Dispenser);
            var nearestDispenser = allPopCornDispensers
                .Where(z =>
                    Distance(
                        actor.World.Engine.GetAbsoluteLocation(actor.ObjectId),
                        actor.World.Engine.GetAbsoluteLocation(z.Item2)) < 20
                    )
                .FirstOrDefault(z => z.Item1.Color == sideColor);
            if (nearestDispenser == null) return null;
            return nearestDispenser.Item2;
        }

        public double TakePopCorn(IActor actor)
        {
            var id = GetMachineId(actor);
            if (id != null)
            {
                //Dispense PopCorn
            }
            return 1.0;
        }
    }
}
