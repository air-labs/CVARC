using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public class EncodersSensor : Sensor<List<EncodersData>,IDWMRobot>
	{
		int pointer = 0;

		public override List<EncodersData> Measure()
		{
			return null;
			//создать лист, переписать туда данные из Actor.DWMData.EncodersHistory начиная с pointer
			pointer = Actor.DWMData.EncodersHistory.Count;
			
		}
	}
}
