using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class CommandFilter<TInputCommand,TOutputCommand,TActor> : ICommandFilter
        where TActor : IActor
        where TInputCommand : ICommand
        where TOutputCommand : ICommand
    {
        public TActor Actor { get; private set; }

        IEnumerable<ICommand> ICommandFilter.Preprocess(IActor _actor, ICommand _command)
        {
            var command = Compatibility.Check<TInputCommand>(this, _command);
			var actor = Compatibility.Check<TActor>(this, _actor);
            return Preprocess(actor,command).Cast<ICommand>();
        }

        public abstract IEnumerable<TOutputCommand> Preprocess(TActor actor, TInputCommand command);

    }
}
