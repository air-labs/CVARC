﻿using System;
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

        RMTestEntry ScoreTest(int scoreCount, params RMCommand[] commands)
        {
            return TestTemplate((data, asserter) => asserter.IsEqual(scoreCount, data.MyScores, 0), commands);
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