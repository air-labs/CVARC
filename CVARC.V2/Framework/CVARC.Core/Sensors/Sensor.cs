using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class Sensor<TMeasurement,TActor> : ISensor
        where TActor : IActor
    {
        public TActor Actor { get; private set; }

        public abstract TMeasurement Measure();


        object ISensor.Measure()
        {
            return Measure();
        }

        public virtual void Initialize(IActor actor)
        {
            Actor = Compatibility.Check<TActor>(this,actor);
        }


        public Type MeasurementType
        {
            get { return typeof(TMeasurement); }
        }
    }
}
