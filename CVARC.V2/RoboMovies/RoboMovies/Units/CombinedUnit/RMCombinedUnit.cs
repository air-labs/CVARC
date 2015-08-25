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
        IRMRobot robot;

        public RMCombinedUnit(IRMRobot actor, RMWorld world) :
            base(actor)
        {
            LeftClapperOffset = new Frame3D(0, 12, 6);
            RightClapperOffset = new Frame3D(0, -12, 6);

            this.world = world;
            this.robot = actor;

            SubUnits.Add("LeftDeployer", x => MakeClapper(x, LeftClapperOffset));
            SubUnits.Add("RightDeployer", x => MakeClapper(x, RightClapperOffset));
            SubUnits.Add("LadderTaker", x => GoStair(x));
            SubUnits.Add("PopcornMachineUser", x => TakePopCorn(x));
        }

        private double Distance(Frame3D a, Frame3D b)
        {
            return AIRLab.Mathematics.Geometry.Distance(a.ToPoint3D(), b.ToPoint3D());
        }

        private IEnumerable<Tuple<RMObject, string>> GetObjectsOfType(ObjectType objectType)
        {
            return world.IdGenerator.GetAllPairsOfType<RMObject>()
                .Where(z => z.Item1.Type == objectType);
        }

        private Tuple<string, SideColor> GetClapperboardToDeploy(IActor actor, Frame3D deployerLocation)
        {
            var clapperboards = GetObjectsOfType(ObjectType.Clapperboard)
                .Select(z => new
                {
                    Id = z.Item2,
                    Dist = Distance(deployerLocation, world.Engine.GetAbsoluteLocation(z.Item2)),
                    Color = z.Item1.Color
                }).ToList();

            Debugger.Log(RMDebugMessage.Logic, String.Format("Found {0} clapperboards:", clapperboards.Count));

            clapperboards = clapperboards
                .Where(z => z.Dist < 10)
                .Where(z => !world.ClosedClapperboards.Contains(z.Id))
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

            var actorLocation = world.Engine.GetAbsoluteLocation(actor.ObjectId);
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
                world.Scores.Add(actor.ControllerId, -10, "Closed opponent's clapperboard");
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
            
            return GetObjectsOfType(ObjectType.Stairs)
                .Where(z => IsPossibleGoUp(world.Engine.GetAbsoluteLocation(actor.ObjectId),
                    world.Engine.GetAbsoluteLocation(z.Item2)))
                .Where(z => z.Item1.Color == actorSideColor)
                .Select(z => z.Item2)
                .FirstOrDefault();
        }

        public double GoStair(IActor actor)
        {
            var stairId = GetStairId(actor);
            if (stairId != null)
            {
                world.Manager.ClimbUpStairs(actor.ObjectId, stairId);
                world.Scores.Add(actor.ControllerId, stairsScore, "Stairs complete.");
                return Double.PositiveInfinity;
            }
            return 1;
        }

        private string GetDispenserId(Frame3D cupLocation)
        {
            return GetObjectsOfType(ObjectType.Dispenser)
                .Where(z => Distance(cupLocation, world.Engine.GetAbsoluteLocation(z.Item2)) < 20)
                .Select(z => z.Item2)
                .FirstOrDefault();
        }

        public double TakePopCorn(IActor actor)
        {
            Debugger.Log(RMDebugMessage.Logic, "Taking pop corn..");

            var cupId = robot.Gripper.GrippedObjectId;
            if (cupId == null) return 1;

            Debugger.Log(RMDebugMessage.Logic, "Cup found!");

            var dispenserId = GetDispenserId(world.Engine.GetAbsoluteLocation(cupId));
            if (dispenserId == null) return 1;
            
            Debugger.Log(RMDebugMessage.Logic, "Dispenser found!");

            if (world.PopCornFullness[dispenserId] > 0 && world.PopCornFullness[cupId] < world.CupCapacity)
            {
                world.PopCornFullness[dispenserId]--;
                world.PopCornFullness[cupId]++;
                Debugger.Log(RMDebugMessage.Logic, "Pop corn successfuly added to cup!");
            }

            return 1;
        }
    }
}
