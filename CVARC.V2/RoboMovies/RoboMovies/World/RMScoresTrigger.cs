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
            UpdatePopCornScores();

            ScheduledTime += Interval;
            return TriggerKeep.Keep;
        }

        void UpdatePopCornScores()
        {
            var popcorn = world.IdGenerator.GetAllPairsOfType<RMObject>()
                .Where(x => x.Item1.Type == ObjectType.PopCorn)
                .Select(x => x.Item2)
                .Where(x => world.Engine.FindParent(x) == null);

            foreach(var popcornId in popcorn)
            {
                var ownerId = world.PopCornOwner[popcornId];
                if (ownerId != null && world.IsCorrectPopCorn(popcornId, ownerId))
                    world.Scores.Add(ownerId, world.PopCornFullness[popcornId],
                        "Popcorn deployed in correct location.", RecordType.Temporary);
            }
        }

        void UpdateStandsScores(string controllerId)
        {
            var color = controllerId == TwoPlayersId.Left ? SideColor.Yellow : SideColor.Green;

            var stands = world.IdGenerator.GetAllPairsOfType<RMObject>()
                .Where(z => z.Item1.Type == ObjectType.Stand && z.Item1.Color == color)
                .Select(z => new { ID = z.Item2, Parent = world.Engine.FindParent(z.Item2) })
                .Where(z => z.Parent == null || world.IdGenerator.KeyOfType<RMObject>(z.Parent))
                .Select(z => world.Engine.GetAbsoluteLocation(z.ID));

            foreach (var standLocation in stands)
                if (world.IsCorrectStand(standLocation, controllerId))
                    world.Scores.Add(controllerId, 2, "Correct stand", RecordType.Temporary);
        }
    }
}
