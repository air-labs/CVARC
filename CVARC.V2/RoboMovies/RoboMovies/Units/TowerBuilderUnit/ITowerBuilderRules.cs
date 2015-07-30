using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface ITowerBuilderRules
    {
        double BuildingTime { get; }
        double CollectingTime { get; }
        int BuilderCapacity { get; }
    }

    public interface ITowerBuilderRules<TCommand> : ITowerBuilderRules
        where TCommand : ITowerBuilderCommand { }
}
