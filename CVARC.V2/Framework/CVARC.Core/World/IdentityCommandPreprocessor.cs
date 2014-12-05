using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class IdentityCommandPreprocessor<TCommand> : CommandPreprocessor<TCommand,TCommand,IActor>
        where TCommand : ICommand
    {
        public override IEnumerable<TCommand> Preprocess(TCommand command)
        {
            yield return command;
        }
    }
}
