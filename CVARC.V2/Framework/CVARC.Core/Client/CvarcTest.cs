using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class CvarcTest<TSensorData,TCommand,TWorld, TWorldState> : ICvarcTest
        where TSensorData : class
        where TWorldState : IWorldState
    {
        public abstract SettingsProposal GetSettings();
        public abstract TWorldState GetWorldState();
        public abstract void Test(CvarcClient<TSensorData, TCommand> client, TWorld world, IAsserter asserter);
     
        public void Run(NetworkServerData holder, IAsserter asserter)
        {
            var client=new CvarcClient<TSensorData,TCommand>();
            var configurationProposal = new ConfigurationProposal
            {
                LoadingData = holder.LoadingData,
                SettingsProposal = GetSettings()
            };
            client.Configurate(holder.Port, configurationProposal, GetWorldState());
            var iworld = holder.WaitForWorld();
            var world = Compatibility.Check<TWorld>(this, iworld);
            Test(client,world,asserter);
        }
    }
}
