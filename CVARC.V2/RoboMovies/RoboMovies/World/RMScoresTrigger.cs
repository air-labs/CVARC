using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using AIRLab.Mathematics;

namespace RoboMovies
{
    public class RMScoresTrigger : Trigger
    {
        RMWorld world;

        public double Interval { get; set; }

        public RMScoresTrigger(RMWorld world, double interval=0.5)
        {
            this.world = world;
            Interval = ScheduledTime = interval;
        }

        public override TriggerKeep Act(double time)
        {
            world.Scores.DeleteTemporaryRecords();

            UpdateStandsScores(TwoPlayersId.Left);
            UpdateStandsScores(TwoPlayersId.Right);
            UpdatePopCornScores(TwoPlayersId.Left);
            UpdatePopCornScores(TwoPlayersId.Right);

            ScheduledTime += Interval;
            return TriggerKeep.Keep;
        }

        void UpdatePopCornScores(string controllerId)
        {
            var popcorn = world.IdGenerator.GetAllPairsOfType<RMObject>()
                .Where(x => x.Item1.Type == ObjectType.PopCorn)
                .Select(x => x.Item2)
                .Where(x => world.Engine.FindParent(x) == null);

            var maxScores = new Dictionary<Cinema, int>();
            var cinema = Cinema.None;

            foreach (var popcornId in popcorn.OrderBy(x => world.PopCornFullness[x]))
                if (world.IsValidPopCorn(popcornId, controllerId, out cinema))
                    maxScores[cinema] = world.PopCornFullness[popcornId];

            foreach (var score in maxScores.Values)
                world.Scores.Add(controllerId, score, 
                    "Popcorn deployed in correct location.", 
                    RecordType.Temporary);
        }

        void UpdateStandsScores(string controllerId)
        {
            var color = controllerId == TwoPlayersId.Left ? SideColor.Yellow : SideColor.Green;

            var stands = world.IdGenerator.GetAllPairsOfType<RMObject>()
                .Where(z => z.Item1.Type == ObjectType.Stand && z.Item1.Color == color)
                .Select(z => new { ID = z.Item2, Parent = world.Engine.FindParent(z.Item2), 
                    Location = world.Engine.GetAbsoluteLocation(z.Item2) })
                .Where(z => z.Parent == null || world.IdGenerator.KeyOfType<RMObject>(z.Parent));

            foreach (var stand in stands)
                if (world.IsValidStand(stand.Location, controllerId))
                    world.Scores.Add(controllerId, 2, "Correct stand", RecordType.Temporary);

            AddBonusForSpotlight(controllerId, f => world.IsInsideBuildingArea(f), s => s.Color == color);
            AddBonusForSpotlight(controllerId, f => world.IsInsideStartingArea(f, color), s => s.Color == color);
        }

        void AddBonusForSpotlight(string controllerId, Func<Frame3D, bool> posFilter, Func<RMObject, bool> standFilter)
        {
            string maxSpotlight = world.Spotlights
                .Where(kv => posFilter(world.Engine.GetAbsoluteLocation(kv.Key)))
                .OrderByDescending(kv => kv.Value.Count)
                .Select(kv => kv.Key)
                .FirstOrDefault();

            if (maxSpotlight != null)
                foreach (var stand in world.Spotlights[maxSpotlight])
                    if (standFilter(world.IdGenerator.GetKey<RMObject>(stand)))
                        world.Scores.Add(controllerId, 3, "Correct spotlight", RecordType.Temporary);
        }
    }
}
