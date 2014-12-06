using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public class LogicPart
    {
        public readonly Dictionary<string, ICvarcTest> Tests = new Dictionary<string, ICvarcTest>();

        public readonly Dictionary<string, Func<IController>> Bots = new Dictionary<string, Func<IController>>();

        public readonly Dictionary<string, ActorFactory> Actors = new Dictionary<string, ActorFactory>();

        public readonly List<string> PredefinedWorldStates = new List<string>();

        public Func<string, IWorldState> CreateWorldState { get; set; }

        public Func<Settings> CreateDefaultSettings { get; set; }

        public Func<IWorld> CreateWorld { get; set; }

        public Type WorldStateType { get; set; }

        public LogicPart()
        {
        }
    }

    
}
