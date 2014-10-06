using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface ISensor
    {
        object Measure();
        void Initialize(IActor actor);
        Type MeasurementType { get; }
    }
}
