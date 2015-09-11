using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace RoboMovies
{
    public partial class RMLogicPartHelper
    {
        [AttributeUsage(AttributeTargets.Method)]
        class TestLoaderMethod : Attribute { }

        RMTestEntry TowerBuilderTest(int count, params RMCommand[] commands)
        {
            return TestTemplate((data, asserter) => asserter.IsEqual(count, data.CollectedDetailsCount, 0), commands);
        }

        RMTestEntry PopCornTest(int count, params RMCommand[] commands)
        {
            return TestTemplate((data, asserter) => asserter.IsEqual(count, data.LoadedPopCornCount, 0), commands);
        }

        RMTestEntry ScoreTest(int scoreCount, params RMCommand[] commands)
        {
            return TestTemplate((data, asserter) => asserter.IsEqual(scoreCount, data.MyScores, 0), commands);
        }

        RMTestEntry PositionTest(double x, double y, double angle, double delta, params RMCommand[] commands)
        {
            return TestTemplate((data, asserter) => { 
                    asserter.IsEqual(x, data.SelfLocation.X, delta);
                    asserter.IsEqual(y, data.SelfLocation.Y, delta);
                    asserter.IsEqual(angle, data.SelfLocation.Angle, delta);
                }, commands);
        }

        RMTestEntry PositionTest(double x, double y, double delta, params RMCommand[] commands)
        {
            return TestTemplate((data, asserter) => { 
                    asserter.IsEqual(x, data.SelfLocation.X, delta);
                    asserter.IsEqual(y, data.SelfLocation.Y, delta);
                }, commands);
        }
        
        RMTestEntry TestTemplate(Action<FullMapSensorData, IAsserter> assert, params RMCommand[] commands)
        {
            return (client, world, asserter) =>
            {
                var data = new FullMapSensorData();
                foreach (var c in commands)
                    data = client.Act(c);
                assert(data, asserter);
            };
        }

        void AddTest(LogicPart logic, string name, RMTestEntry test)
        {
            logic.Tests[name] = new RMTestBase(test, new RMWorldState());
        }
    }
}
