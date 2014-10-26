using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class CommandPreprocessor<TCommand,TActor> : ICommandPreprocessor
        where TCommand : ICommand
        where TActor : IActor
    {
        public TActor Actor { get; private set; }

        IEnumerable<ICommand> ICommandPreprocessor.Preprocess(object command)
        {
            return Preprocess(command).Cast<ICommand>();
        }

        public abstract IEnumerable<TCommand> Preprocess(object command);

        public void Initialize(IActor actor)
        {
            Actor=Compatibility.Check<TActor>(this, actor);
        }
    }
}
