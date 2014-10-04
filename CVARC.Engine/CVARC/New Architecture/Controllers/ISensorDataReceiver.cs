using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public interface IReactiveController<in TSensorsData, out TCommand> : IController<TCommand>
        where TSensorsData : ISensorsData
        where TCommand : ICommand
    {
        void AcceptSensorsData(TSensorsData sensorsData);
    }
}
