using System;
using System.Reflection;
using CVARC.Graphics;

namespace CVARC.Basic.Sensors
{
    public class Sensor
    {
        private readonly MethodInfo _meas;
        private readonly object _sens;
        public string Name { get; set; }
        public Sensor(Type @from, Robot robot, World root, DrawerFactory factory)
        {
            var constr = @from.GetConstructor(new Type[0]);
                if (constr == null) return;
                var init = @from.GetMethod("Init");
                if (init == null) return;
            _sens = constr.Invoke(new object[0]);
            init.Invoke(_sens, new object[]
                    {
                        robot, root, factory
                    });
            _meas = @from.GetMethod("Measure");
        }
        public string Measure()
        {
            var r = _meas.Invoke(_sens, new object[0]);
            if (r == null) return "";
            if (r.GetType().GetInterface("ISensorData") != null)
                return r.GetType().GetMethod("GetStringRepresentation").Invoke(r, new object[0]).ToString();
            return "";
        }
    }
}
