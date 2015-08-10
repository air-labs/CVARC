using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
    public class DWMRobot : Robot<IActorManager,DWMWorld,DWMSensorsData,DWMCommand,DWMRules>,
                IDWMRobot
    {
		public DWMUnit DWMUnit { get; private set;  }

		public override IEnumerable<IUnit> Units
		{
			get
			{
				yield return DWMUnit;
			}
		}
	
		public override void AdditionalInitialization()
		{
			base.AdditionalInitialization();
			DWMUnit = new DWMUnit(this);
			DWMData = new DWMData();
			var state = World.WorldState;
			// в этом state есть настройки шума, которые надо перенести в GAX
			var gax = sensors.Sensors.OfType<GAXSensor>().FirstOrDefault();
			//gax.AccelerometerDistortion = state.AccelerometerDistortion;
			var filter = this.FilterSet.Filters.OfType<DWMDistortionCommandFilter>().FirstOrDefault();
			filter.Multiplier = 1+state.ControlDistortion;
		}

        public DWMData DWMData
        {
            get;
            set;
        }
    }
}
