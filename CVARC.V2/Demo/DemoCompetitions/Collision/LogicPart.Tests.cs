using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2.SimpleMovement;
using CVARC.V2;

namespace Demo.Collision
{
    public partial class CollisionLogicPart
    {
        CollisionTestEntry LocationTest(int count, int left, int right, params SimpleMovementCommand[] command)
        {
            return (client, world, asserter) =>
            {
                int counter = 0;
                int lscore = 0;
                int rscore = 0;
                world.Scores.ScoresChanged += () => {counter++;};
                foreach (var c in command)
                    client.Act(c);
                asserter.IsEqual(count, counter,0);
                foreach (var item in world.Scores.Records)
                {
                    if (item.Key == "Left")
                        lscore = item.Value.Count;
                    if (item.Key == "Right")
                        rscore = item.Value.Count;
                }
                asserter.IsEqual(lscore, left, 0);
                asserter.IsEqual(rscore, right, 0);
            };
        }


        private void LoadTests()
        {
            Tests["NoCollision"] = new CollisionTestBase(LocationTest(0, 0, 0,
                SimpleMovementCommand.Action("Grip"),
                SimpleMovementCommand.Move(-50, 0.2),
                SimpleMovementCommand.Move(50, 0.2)), true);
            Tests["CollisionCount"] = new CollisionTestBase(LocationTest(3, 3, 0,
                SimpleMovementCommand.Action("Grip"),
                SimpleMovementCommand.Move(50, 1),
                SimpleMovementCommand.Rotate(Angle.HalfPi, 1),
                SimpleMovementCommand.Move(50, 0.8),
                SimpleMovementCommand.Move(50, 0.3),
                SimpleMovementCommand.Move(-50, 0.2),
                SimpleMovementCommand.Move(50, 0.3),
                SimpleMovementCommand.Move(-50, 0.2),
                SimpleMovementCommand.Move(50, 0.3),
                SimpleMovementCommand.Move(-50, 0.2)), true);
            Tests["CollisionBox"] = new CollisionTestBase(LocationTest(1, 1, 0,
                SimpleMovementCommand.Action("Grip"),
                SimpleMovementCommand.Move(50, 1.5),
                SimpleMovementCommand.Rotate(Angle.HalfPi, 1),
                SimpleMovementCommand.Move(50, 0.45)), true);
            Tests["NoBox"] = new CollisionTestBase(LocationTest(3, 3, 0,
                SimpleMovementCommand.Move(50, 1.05),
                SimpleMovementCommand.Rotate(Angle.HalfPi, 1),
                SimpleMovementCommand.Move(50, 0.8),
                SimpleMovementCommand.Move(50, 0.3),
                SimpleMovementCommand.Move(-50, 0.2),
                SimpleMovementCommand.Move(50, 0.3),
                SimpleMovementCommand.Move(-50, 0.2),
                SimpleMovementCommand.Move(50, 0.3),
                SimpleMovementCommand.Move(-50, 0.2)), true);
            Tests["RotateNoBox"] = new CollisionTestBase(LocationTest(1, 1, 0,
                SimpleMovementCommand.Move(50, 1.7),
                SimpleMovementCommand.Rotate(Angle.HalfPi, 1),
                SimpleMovementCommand.Move(50, 1.45),
                SimpleMovementCommand.Rotate(Angle.HalfPi, 1),
                SimpleMovementCommand.Move(50, 0.7),
                SimpleMovementCommand.Rotate(Angle.HalfPi, 1)), true);
            Tests["RotateBox"] = new CollisionTestBase(LocationTest(1, 1, 0,
                SimpleMovementCommand.Action("Grip"),
                SimpleMovementCommand.Move(50, 1.7),
                SimpleMovementCommand.Rotate(Angle.HalfPi, 1),
                SimpleMovementCommand.Move(50, 1.6),
                SimpleMovementCommand.Rotate(Angle.HalfPi, 1),
                SimpleMovementCommand.Move(50, 0.7),
                SimpleMovementCommand.Rotate(Angle.HalfPi, 1)), true);



        }
    }
}
