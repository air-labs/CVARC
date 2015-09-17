using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class CommandFilterSet
    {
        public readonly List<ICommandFilter> Filters = new List<ICommandFilter>();

        IEnumerator<ICommand> currentEnumerator;
        public bool CommandAvailable { get; private set; }

        public void ProcessCommand(IActor actor, ICommand command)
        {
            IEnumerable<ICommand> result = new List<ICommand> { command };
            foreach (var e in Filters)
                result = result.SelectMany(c => e.Preprocess(actor,c));
            currentEnumerator = result.GetEnumerator();
            CommandAvailable = currentEnumerator.MoveNext();
        }

        public ICommand GetNextCommand()
        {
            if (!CommandAvailable) throw new Exception("Command is not available, but was requested");
            var cmd = currentEnumerator.Current;
            CommandAvailable = currentEnumerator.MoveNext();
            return cmd;
        }
    }
}
