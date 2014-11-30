using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class DelegatedCvarcTest<TSensorData, TCommand, TWorld, TWorldState> : CvarcTest<TSensorData, TCommand, TWorld, TWorldState>
        where TSensorData : class
        where TWorldState : IWorldState
    {

        Action<CvarcClient<TSensorData,TCommand>,TWorld, IAsserter> test;
        public override void Test(CvarcClient<TSensorData, TCommand> client, TWorld world, IAsserter asserter)
        {
            test(client, world, asserter);
        }
        public DelegatedCvarcTest(Action<CvarcClient<TSensorData,TCommand>,TWorld,IAsserter> test)
        {
            this.test = test;
        }
    }
}
