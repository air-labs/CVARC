using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface ICommandPreprocessor
    {
        IEnumerable<ICommand> Preprocess(ICommand command);
        void Initialize(IActor actor);
    }
}
