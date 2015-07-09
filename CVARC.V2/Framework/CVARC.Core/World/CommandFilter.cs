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

        IEnumerable<ICommand> ICommandFilter.Preprocess(ICommand _command)
        {
            var command = Compatibility.Check<TInputCommand>(this, _command);
            return Preprocess(command).Cast<ICommand>();
        }

        public abstract IEnumerable<TOutputCommand> Preprocess(TInputCommand command);

        public void Initialize(IActor actor)
        {
            Actor=Compatibility.Check<TActor>(this, actor);
        }
    }
}
