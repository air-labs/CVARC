using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class CommandPreprocessor<TExternalCommand,TInternalCommand,TActor> : ICommandPreprocessor
        where TActor : IActor
        where TExternalCommand : ICommand
        where TInternalCommand : ICommand
    {
        public TActor Actor { get; private set; }

        IEnumerable<ICommand> ICommandPreprocessor.Preprocess(ICommand _command)
        {
            var command = Compatibility.Check<TExternalCommand>(this, _command);
            return Preprocess(command).Cast<ICommand>();
        }

        public abstract IEnumerable<TInternalCommand> Preprocess(TExternalCommand command);

        public void Initialize(IActor actor)
        {
            Actor=Compatibility.Check<TActor>(this, actor);
        }
    }
}
