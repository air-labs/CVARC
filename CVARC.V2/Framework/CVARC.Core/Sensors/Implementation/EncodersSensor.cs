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
			
			//создать лист, переписать туда данные из Actor.DWMData.EncodersHistory начиная с pointer
            List<EncodersData> encoderRecords = new List<EncodersData>();
            encoderRecords.AddRange(Actor.DWMData.EncodersHistory.Skip(pointer));
			pointer = Actor.DWMData.EncodersHistory.Count;
            return encoderRecords;
		}
	}
}
