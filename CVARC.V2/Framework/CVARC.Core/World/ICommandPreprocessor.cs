using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface ICommandPreprocessor
    {
        IEnumerable<ICommand> Preprocess(object command);
        void Initialize(IActor actor);
    }
}
