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
        Frame3D LeftClapperPoint;
        Frame3D RightClapperPoint;

        public RMCombinedUnit(IActor actor) :
            base(actor)
        {
            LeftClapperPoint = new Frame3D(-5,0,0);
            RightClapperPoint = new Frame3D(5,0,0);

            SubUnits.Add("LeftDeployer", x => MakeClapper(x, LeftClapperPoint));
            SubUnits.Add("RightDeployer", x => MakeClapper(x, RightClapperPoint));
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

        private Tuple<string, SideColor> GetClapperboardToDeploy(IActor actor, Frame3D point)
        {
            var clapperboards = GetObjectsOfType(actor, ObjectType.Clapperboard);
            var nearerClapperboards = clapperboards
                .Select(z => new
                {
                    Id = z.Item2,
                    Dist = Distance(point + (actor.World.Engine.GetAbsoluteLocation(actor.ObjectId)),
                                    actor.World.Engine.GetAbsoluteLocation(z.Item2)),
                    Color = z.Item1.Color
                })
                .Where(z => z.Dist<15)
                .Where(z =>
                    {
                        var world = actor.World as RMWorld;
                        if (!(world.IsClapperboardClosed.ContainsKey(z.Id)))
                            throw new ArgumentException("id хлопушки не найден в словаре закрытия/открытия");
                        return !world.IsClapperboardClosed[z.Id];
                    })
                .ToList();
            if (nearerClapperboards.Count == 0) return null;
            return new Tuple<string, SideColor>(nearerClapperboards[0].Id, nearerClapperboards[0].Color);
        }

        public double MakeClapper(IActor actor, Frame3D point)
        {
            var clapperboardToDeploy = GetClapperboardToDeploy(actor, point);
            if (clapperboardToDeploy == null) return 1;

            var currentWorldManager = actor.World.Manager as IRMWorldManager;
            currentWorldManager.CloseClapperboard(clapperboardToDeploy.Item1);

            var world = actor.World as RMWorld;
            world.IsClapperboardClosed[clapperboardToDeploy.Item1] = true;

            var sideId = clapperboardToDeploy.Item2 == SideColor.Yellow ? TwoPlayersId.Left : TwoPlayersId.Right;
            actor.World.Scores.Add(sideId, 10, "clapperboardDeploy");
            return 1;
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
                actor.World.Scores.Add(actor.ControllerId, 50, "ladderComplete");
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
