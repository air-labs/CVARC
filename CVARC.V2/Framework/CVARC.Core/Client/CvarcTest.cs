using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class CvarcTest<TSensorData,TCommand,TWorld> : ICvarcTest
        where TSensorData : class
    {
        public abstract ConfigurationProposal GetConfiguration();
        public abstract void Test(CvarcClient<TSensorData, TCommand> client, TWorld world, IAsserter asserter);
     
        public void Run(int port, IWorldHolder holder, IAsserter asserter)
        {
            var client=new CvarcClient<TSensorData,TCommand>();
            client.Configurate(false, port, GetConfiguration());
            var iworld = holder.RetrieveWorld();
            var world = Compatibility.Check<TWorld>(this, iworld);
            Test(client,world,asserter);
        }
    }
}
